using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Extensions.Logging;
using RestSharp;
using VersionObserver.Models;
using VersionObserver.Services;

namespace VersionObserver.Services
{
    public interface IObserverService
    {
        IEnumerable<string> GetRepos(string reposURI);
        IEnumerable<string> GetItemsFromRepo(string repoURI);
        string GetTreeURIFromItem(string itemURI);
        TreeObject.Rootobject GetTreeObjectFromTree(string treeURI);
    }
    public class ObserverService : IObserverService
    {
        private IAzureDevOpsApiProxyService _apiProxy;
        ILogger<ObserverService> _logger;

        public ObserverService(ILogger<ObserverService> logger, IAzureDevOpsApiProxyService apiProxy)
        {
            _apiProxy = apiProxy;
            _logger = logger;
        }

        public IEnumerable<string> GetRepos(string reposURI)
        {
            List<string> result;
            IRestResponse response;
            _apiProxy.CallAzureDevOpsAPI(reposURI, out result, out response);

            var responseObject = JsonSerializer.Deserialize<Rootobject>(response.Content);

            foreach (var value in responseObject.value)
            {
                result.Add(value.url);
            }

            return result;
        }
        public IEnumerable<string> GetItemsFromRepo(string repoURI)
        {
            var result = new List<string>();
            IRestResponse response;
            _apiProxy.CallAzureDevOpsAPI(repoURI + "/items", out result, out response);

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
            IRestResponse response;
            _apiProxy.CallAzureDevOpsAPI(itemURI, out result, out response);

            var item = JsonSerializer.Deserialize<ItemRootobject>(response.Content);

            return item._links.tree.href;
        }

            public TreeObject.Rootobject GetTreeObjectFromTree(string treeURI)
        {
            var result = new List<string>();
            TreeObject.Rootobject responseObject = null;
            try
            {
                IRestResponse response;
                _apiProxy.CallAzureDevOpsAPI(treeURI + "?recursive=true&api-version=6.0", out result, out response);

                responseObject = JsonSerializer.Deserialize<TreeObject.Rootobject>(response.Content);
            }
            catch
            {
                _logger.LogWarning("Could not get repository tree" + treeURI);
            }
            return responseObject;
        }


        public IEnumerable<ProjectFileMetaData> GetCSProjIfPresentInTree(TreeObject.Rootobject treeRoots)
        {
            var results = new List<ProjectFileMetaData>();
            if (treeRoots != null && treeRoots.treeEntries != null) { 
            foreach (var treeItem in treeRoots.treeEntries)
            {
                if (treeItem.relativePath.Contains("csproj"))
                {
                    results.Add(new ProjectFileMetaData() { projectName = treeItem.relativePath, projectURL = treeItem.url });
                }
            } }
            return results;
        }


        public XmlDocument GetCSProjBlobAsync(string blobURL)
        {
            try
            {
                IRestResponse response = _apiProxy.GetCSProjBlobXML(blobURL);
                Console.WriteLine(response.Content);

                //Ensuring correct xml structure needs a little hand holding.
                string x = "";
                var bytes = Encoding.ASCII.GetBytes(response.Content);
                foreach (var b in bytes)
                {
                    x += (char)b;
                }
                x = x.Replace("?", "");

                XmlDocument result = new XmlDocument();
                result.LoadXml(x);
                return result;
            }
            catch (Exception e)
            {
                _logger.LogWarning("Unable to retrieve csproj blob object" + blobURL);
                _logger.LogWarning(e.InnerException.ToString());
            }
            return null;
        }

    }
}
