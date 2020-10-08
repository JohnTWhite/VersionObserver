using System;
using System.Collections.Generic;
using System.Xml;
using VersionObserver.Models;

namespace VersionObserver
{
    public class ObserverServiceFacade
    {
        ObserverService _service;
        const string DEVOPSURI = "https://dev.azure.com/ClearentTFS/Clearent/_apis/git/repositories";
        public ObserverServiceFacade(ObserverService service)
        {
            _service = service;
        }

        /// <summary>
        /// Facade that calls underlying service to
        /// return CSProj xml files from Azure Devops api
        /// </summary>
        /// <returns></returns>
        public IEnumerable<CSProjFile> GetAzureDevOpsCSProjFiles()
        {

            var reposURIs = _service.GetRepos(DEVOPSURI);

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
