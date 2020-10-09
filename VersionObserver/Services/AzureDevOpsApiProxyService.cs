using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using VersionObserver.Models;

namespace VersionObserver.Services
{
    public interface IAzureDevOpsApiProxyService
    {
        void CallAzureDevOpsAPI(string reposURI, out List<string> result, out IRestResponse response);
        IRestResponse GetCSProjBlobXML(string blobURL);
    }
    public class AzureDevOpsApiProxyService : IAzureDevOpsApiProxyService
    {
        private string _authToken;
        private string _cookies;
        private string _authWithBearer;

        public AzureDevOpsApiProxyService(AzureDevOpsApiConfiguration configuration)
        {
            _authToken = configuration.AuthToken;
            _cookies = configuration.Cookies;
            _authWithBearer = string.Join("Basic ", _authToken);
        }

        public void CallAzureDevOpsAPI(string reposURI, out List<string> result, out IRestResponse response)
        {
            result = new List<string>();
            var client = new RestClient(reposURI);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);

            request.AddHeader("Authorization", _authWithBearer);
            request.AddHeader("Cookie", _cookies);
            response = client.Execute(request);
            //return response
        }
        public IRestResponse GetCSProjBlobXML(string blobURL)
        {
            var client = new RestClient(blobURL);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Accept", "application/xml");
            request.AddHeader("Authorization", _authWithBearer);
            request.AddHeader("Cookie", _cookies);
            IRestResponse response = client.Execute(request);
            return response;
        }
    }
}
