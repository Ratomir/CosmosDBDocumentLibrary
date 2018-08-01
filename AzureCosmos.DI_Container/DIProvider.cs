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

        protected  DIProvider()
        {
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

                ConfigurationBuilder confBuild = new ConfigurationBuilder();

                IConfiguration config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", true, true)
                    .Build();

                collection.AddSingleton(config);

                serviceProvider = collection.BuildServiceProvider();
            }

            return serviceProvider;
        }
    }
}
