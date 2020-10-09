using System;
using System.Collections.Generic;
using System.Xml;
using VersionObserver.Models;
using VersionObserver.Services;

namespace VersionObserver
{
    public class ObserverServiceFacade
    {
        private IObserverService _service;
        private string _devOpsApiUrl;
        public ObserverServiceFacade(IObserverService service, AzureDevOpsApiConfiguration apiConfigurations)
        {
            _service = service;
            _devOpsApiUrl = apiConfigurations.BaseURL;
        }

        /// <summary>
        /// Facade that calls underlying service to
        /// return CSProj xml files from Azure Devops api
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CSProjFile> GetAzureDevOpsCSProjFiles()
        {

            var reposURIs = _service.GetRepos(_devOpsApiUrl);

            IList<CSProjFile> results = new List<CSProjFile>();
            foreach (var repo in reposURIs)
            {
                var items = _service.GetItemsFromRepo(repo);
                foreach(var item in items)
                {
                    var treeURI = _service.GetTreeURIFromItem(item);
                    var treeObj = _service.GetTreeObjectFromTree(treeURI);
                    
                    var projFiles = _service.GetCSProjIfPresentInTree(treeObj);
                    foreach(var projectFile in projFiles)
                    {
                        var result = _service.GetCSProjBlobAsync(projectFile.projectURL);
                        results.Add(new CSProjFile() { ProjectName = projectFile.projectName,xmlDocument = result });
                    }
                }
            }
            return results;
        }
    }
}
