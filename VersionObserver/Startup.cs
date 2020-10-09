using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.IO;
using VersionObserver.Models;
using VersionObserver.Services;
using Microsoft.Extensions.Logging;

namespace VersionObserver
{
    public static class Startup
    {
        public static IServiceProvider ConfigureServices()
        {
            //Moving from out of the compiled bin/debug/netcore folder into the solution level.
            var parentDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent;
            //to retrieve config files, in an enviroment agnostic way. 
            var configFilePath = Path.Combine(parentDirectory.FullName,"configs", "VersionObserver.json");
            var Configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile(configFilePath, optional: false, reloadOnChange: true)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .CreateLogger();

            return ConfigureServices(Configuration);
        }

        public static IServiceProvider ConfigureServices(IConfiguration services)
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddLogging(sc => sc.AddConsole());

            var apiConfiguration = new AzureDevOpsApiConfiguration();
            services.GetSection("AzureDevOpsApiConfiguration").Bind(apiConfiguration, c => c.BindNonPublicProperties = true);
            serviceCollection.AddSingleton<AzureDevOpsApiConfiguration>(sc => apiConfiguration);

            serviceCollection.AddSingleton<FileService>(sc => new FileService());
            serviceCollection.AddSingleton<IAzureDevOpsApiProxyService, AzureDevOpsApiProxyService>();
            serviceCollection.AddSingleton<ObserverService>();
            serviceCollection.AddSingleton<ObserverServiceFacade>();

            return serviceCollection.BuildServiceProvider();
        }
    }
}
