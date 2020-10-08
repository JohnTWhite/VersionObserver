using System;
using System.Collections.Generic;
using System.Xml;
using VersionObserver.Models;

namespace VersionObserver
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Starting the OBSERVSTION");

            //Collect CSProj's with project dependencies
            var fileService = new FileService();
            var serviceFacade = new ObserverServiceFacade(new ObserverService());
            IEnumerable<CSProjFile> csprojFiles;

            if (args.Length > 0)
            {
                csprojFiles = fileService.GetCSProjFiles(args[0]);
            }
            else
            {
                csprojFiles = serviceFacade.GetAzureDevOpsCSProjFiles();
            }

            //Format data to object with "Project" "Package" "Version" properties.
            IEnumerable<DependencyInformation> dependencies = fileService.GetDependenciesFromProjectFiles(csprojFiles);

            //Save Data to Dedicated database. 
            fileService.SaveDependencies(dependencies);

            Console.WriteLine("Ending the OBSERVSTION");
        }
    }
}
