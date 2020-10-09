using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Xml;
using VersionObserver.Models;
using VersionObserver.Services;

namespace VersionObserver
{
    class Program
    {
        static void Main(string[] args)
        {
            

            var serviceProvider = Startup.ConfigureServices();
            var logger = serviceProvider.GetService<ILogger<Program>>();
            var fileService = serviceProvider.GetRequiredService<FileService>();
            var serviceFacade = serviceProvider.GetRequiredService<ObserverServiceFacade>();

            try
            {
                logger.LogInformation("STARTING the OBSERVSTION");

                //Collect CSProj's with project dependencies
                IEnumerable<CSProjFile> csprojFiles;

                //When an argument is present it is assumed to be a file path on disc that should be
                //recurssively searched for all CSProj files. This behavior is substituted for AzureDevOps API search.
                if (args.Length > 0)
                {
                    logger.LogInformation($"BEGIN {nameof(FileService.GetCSProjFiles)} with Arguments {args[0]}");
                    csprojFiles = fileService.GetCSProjFiles(args[0]);
                    logger.LogInformation($"END {nameof(FileService.GetCSProjFiles)} with Arguments {args[0]}");
                }
                else
                {
                    logger.LogInformation($"BEGIN {nameof(ObserverServiceFacade.GetAzureDevOpsCSProjFiles)}");
                    csprojFiles = serviceFacade.GetAzureDevOpsCSProjFiles();
                    logger.LogInformation($"END {nameof(ObserverServiceFacade.GetAzureDevOpsCSProjFiles)}");
                }

                //Format data to object with "Project" "Package" "Version" properties.
                logger.LogInformation($"BEGIN {nameof(FileService.GetDependenciesFromProjectFiles)}");
                IEnumerable<DependencyInformation> dependencies = fileService.GetDependenciesFromProjectFiles(csprojFiles);
                logger.LogInformation($"END {nameof(FileService.GetDependenciesFromProjectFiles)}");

                //Save Data to Dedicated database. 
                logger.LogInformation($"BEGIN {nameof(FileService.SaveDependencies)}");
                fileService.SaveDependencies(dependencies);
                logger.LogInformation($"END {nameof(FileService.SaveDependencies)}");

                logger.LogInformation("STOPPING the OBSERVSTION");
            }
            catch(Exception e)
            {
                logger.LogError("Unable to complete process due to exception: ", new { ExceptionMessage = e });
            }
        }
    }
}
