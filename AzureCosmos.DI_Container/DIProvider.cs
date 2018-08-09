using AzureCosmosCore.Interface;
using AzureCosmosCore.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.IO;

namespace AzureCosmosCore.DI_Container
{
    public class DIProvider
    {
        public static ServiceProvider ServiceProvider;

        public string JsonFileConfigurationLocation { get; set; }
        public string AppDirectory { get; set; }

        public ServiceCollection Collection { get; set; }

        public IConfiguration Config { get; set; }

        public DIProvider(IConfiguration config)
        {
            Config = config;

            BuildServiceProvider();
        }

        public DIProvider(string jsonFileConfigurationLocation, string appDirectory, IConfiguration config = null)
        {
            JsonFileConfigurationLocation = jsonFileConfigurationLocation;
            AppDirectory = appDirectory;
            Config = config;

            BuildServiceProvider();
        }

        public static ServiceProvider GetServiceProvider() => ServiceProvider;

        public void BuildServiceProvider()
        {
            Collection = new ServiceCollection();
            Collection.AddTransient<IBaseRepository, BaseDatabaseRepository>();
            Collection.AddTransient<ISQLCollectionRepository, SQLCollectionRepository>();
            Collection.AddTransient<ISQLDatabaseRepository, SQLDatabaseRepository>();
            Collection.AddTransient<ISQLDocumentRepository, SQLDocumentRepository>();
            Collection.AddTransient<ISQLStoredProcedureRepository, SQLStoredProcedureRepository>();
            Collection.AddTransient<ISQLTriggerRepository, SQLTriggerRepository>();
            Collection.AddTransient<ISQLUDFRepository, SQLUDFRepository>();

            if (Config == null)
            {
                Config = new ConfigurationBuilder()
                .SetBasePath(AppDirectory)
                .AddJsonFile(JsonFileConfigurationLocation, optional: true, reloadOnChange: true)
                .Build();
            }

            Collection.AddSingleton(Config);

            ServiceProvider = Collection.BuildServiceProvider();
        }

        public static void SetServiceProvider(ServiceProvider customServiceProvider)
        {
            ServiceProvider = customServiceProvider;
        }
    }
}
