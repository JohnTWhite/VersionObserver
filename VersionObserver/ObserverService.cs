using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using RestSharp;
using VersionObserver.Models;

namespace VersionObserver
{
    public class ObserverService
    {
        private string _authToken;
        private string _cookies;
        private string _authWithBearer;

        public ObserverService(string authToken, string cookies)
        {
            _authToken = authToken;
            _cookies = cookies;
            _authWithBearer = string.Join("Basic ", _authToken);
        }

        public IEnumerable<string> GetRepos(string reposURI)
        {
            var result =  new List<string>();
            var client = new RestClient(reposURI);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);

            request.AddHeader("Authorization", _authWithBearer);
            request.AddHeader("Cookie", _cookies);
            IRestResponse response = client.Execute(request);

            var responseObject = JsonSerializer.Deserialize<Rootobject>(response.Content);

            foreach(var value in responseObject.value)
            {
                result.Add( value.url);
            }

            return result;
        }

        public IEnumerable<string> GetItemsFromRepo(string repoURI)
        {
            var result = new List<string>();
            var client = new RestClient(repoURI + "/items");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", _authWithBearer);
            request.AddHeader("Cookie", _cookies);
            IRestResponse response = client.Execute(request);

            var responseObject = JsonSerializer.Deserialize<ItemsRootobject>(response.Content);

            if (responseObject != null && responseObject.value != null)
            {
                foreach (var value in responseObject.value)
                {
                    result.Add(value.url);
                }
            }

            return result;
        }


        public string GetTreeURIFromItem(string itemURI)
        {
            var result = new List<string>();
            var client = new RestClient(itemURI);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", _authWithBearer);
            request.AddHeader("Cookie", _cookies);
            IRestResponse response = client.Execute(request);

            var item = JsonSerializer.Deserialize<ItemRootobject>(response.Content);

            return item._links.tree.href;
        }

            public TreeObject.Rootobject GetTreeObjectFromTree(string treeURI)
        {
            var result = new List<string>();
            TreeObject.Rootobject responseObject = null;
            try
            {
                var client = new RestClient(treeURI + "?recursive=true&api-version=6.0");
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("Authorization", _authWithBearer);
                request.AddHeader("Cookie", _cookies);
                IRestResponse response = client.Execute(request);

                responseObject = JsonSerializer.Deserialize<TreeObject.Rootobject>(response.Content);
            }
            catch
            {
                Console.WriteLine("Couldn't Get Tree" + treeURI);
            }
            return responseObject;
        }

        public class Project
        {
            public string projectURL { get; set; }
            public string projectName { get; set; }
        }


        public IEnumerable<Project> GetCSProjIfPresentInTree(TreeObject.Rootobject treeRoots)
        {
            var results = new List<Project>();
            if (treeRoots != null && treeRoots.treeEntries != null) { 
            foreach (var treeItem in treeRoots.treeEntries)
            {
                if (treeItem.relativePath.Contains("csproj"))
                {
                    results.Add(new Project() { projectName = treeItem.relativePath, projectURL = treeItem.url });
                }
            } }
            return results;
        }


        public XmlDocument GetCSProjBlobAsync(string blobURL)
        {
            try
            {
                var client = new RestClient(blobURL);
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("Accept", "application/xml");
                request.AddHeader("Authorization", _authWithBearer);
                request.AddHeader("Cookie", _cookies);
                IRestResponse response = client.Execute(request);
                Console.WriteLine(response.Content);

                //Ensuring correct xml structure needs a little hand holding.
                string x = "";
                var bytes = Encoding.ASCII.GetBytes(response.Content);
                foreach(var b in bytes){
                        x += (char)b;
                }
                x = x.Replace("?", "");

                XmlDocument result = new XmlDocument();
                result.LoadXml(x);
                return result;
            }
            catch(Exception e)
            {
                Console.WriteLine("Unable to retrieve csproj blob object" + blobURL);
                Console.WriteLine(e);
            }
            return null;
        }
    }
}
