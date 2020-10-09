using Microsoft.Extensions.Logging;
using Moq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using VersionObserver.Models;
using VersionObserver.Services;
using Xunit;

namespace VersionObserver.Tests
{
    
    public class ObserverService_UT
    {
        ObserverService _testObject;
        Mock<ILogger<ObserverService>> _logger;
        Mock<IAzureDevOpsApiProxyService> _apiProxy;
        IRestResponse _response;
        List<string> _result;

        public ObserverService_UT()
        {
            _logger = new Mock<ILogger<ObserverService>>();
            _apiProxy = new Mock<IAzureDevOpsApiProxyService>();
            _response = new RestResponse();
            _result = new List<string>();
            _testObject = new ObserverService(_logger.Object, _apiProxy.Object, _result, _response);
        }

        [Fact]
        public void GetReposCallsApiProxy()
        {
            //arrange
            var endpointURI = "www.azure.com/fake/api/repos";
            var nextObjectURL = "www.azure.com/fake/api/repos/childobject";
            var responseObject = new Rootobject() { value = new Value[1] { new Value() { url = nextObjectURL} } };
            _response.Content = JsonSerializer.Serialize(responseObject);
            _apiProxy.Setup(a => a.CallAzureDevOpsAPI(endpointURI, out _result, out _response));

            //act
            var result = _testObject.GetRepos(endpointURI);

            //assert
            Assert.Contains(nextObjectURL, result);
        }

        [Fact]
        public void GetItemsFromRepoCallsApiProxy()
        {
            //arrange
            var endpointURI = "www.azure.com/fake/api/repos/items";
            var nextObjectURL = "www.azure.com/fake/api/repos/items/childobject";
            var responseObject = new ItemsRootobject() { value = new Value[1] { new Value() { url = nextObjectURL } } };
            _response.Content = JsonSerializer.Serialize(responseObject);
            _apiProxy.Setup(a => a.CallAzureDevOpsAPI(endpointURI + "/items", out _result, out _response));

            //act
            var result = _testObject.GetItemsFromRepo(endpointURI);

            //assert
            Assert.Contains(nextObjectURL, result);
        }

        [Fact]
        public void GetTreeURIFromItemCallsApiProxy()
        {
            //arrange
            var endpointURI = "www.azure.com/fake/api/repos/items";
            var nextObjectURL = "www.azure.com/fake/api/repos/items/childobject";
            var responseObject = new ItemRootobject()
            {
                _links = new _Links()
                {
                    tree = new Tree()
                    {
                        href = nextObjectURL
                    }
                }
            };
            _response.Content = JsonSerializer.Serialize(responseObject);
            _apiProxy.Setup(a => a.CallAzureDevOpsAPI(endpointURI, out _result, out _response));

            //act
            var result = _testObject.GetTreeURIFromItem(endpointURI);

            //assert
            Assert.Contains(nextObjectURL, result);
        }
    }
}
