using AzureCosmosCore.Interface;
using AzureCosmosCore.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace AzureCosmosCore.DI_Container
{
    public class DIProvider
    {
        private static ServiceProvider serviceProvider = null;

        public static string JsonFileConfigurationLocation { get; set; } = "appsettings.json";
        public static string AppDirectory { get; set; } = Directory.GetCurrentDirectory();

        public DIProvider(string jsonFileConfigurationLocation)
        {
            JsonFileConfigurationLocation = jsonFileConfigurationLocation;
        }

        public DIProvider(string jsonFileConfigurationLocation, string appDirectory)
        {
            JsonFileConfigurationLocation = jsonFileConfigurationLocation;
            AppDirectory = appDirectory;
        }

        public static ServiceProvider GetServiceProvider()
        {
            if (serviceProvider == null)
            {
                ServiceCollection collection = new ServiceCollection();
                collection.AddTransient<IBaseRepository, BaseDatabaseRepository>();
                collection.AddTransient<ISQLCollectionRepository, SQLCollectionRepository>();
                collection.AddTransient<ISQLDatabaseRepository, SQLDatabaseRepository>();
                collection.AddTransient<ISQLDocumentRepository, SQLDocumentRepository>();
                collection.AddTransient<ISQLStoredProcedureRepository, SQLStoredProcedureRepository>();
                collection.AddTransient<ISQLTriggerRepository, SQLTriggerRepository>();
                collection.AddTransient<ISQLUDFRepository, SQLUDFRepository>();

                IConfiguration config = new ConfigurationBuilder()
                    .SetBasePath(AppDirectory)
                    .AddJsonFile(JsonFileConfigurationLocation, optional: true, reloadOnChange: true)
                    .Build();

                collection.AddSingleton(config);

                serviceProvider = collection.BuildServiceProvider();
            }

            return serviceProvider;
        }

        public static void SetServiceProvider(ServiceProvider customServiceProvider)
        {
            serviceProvider = customServiceProvider;
        }
    }
}
