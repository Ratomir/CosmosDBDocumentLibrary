using AzureCosmosCore.DI_Container;
using AzureCosmosCore.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace AzureCosmosCore.xUnitTest
{
    public class DI_Container_Test
    {
        [Fact]
        public void Test1()
        {
            DIProvider.JsonFileConfigurationLocation = "appsettings - Copy.json";
            IConfiguration configuration = DIProvider.GetServiceProvider().GetService<IConfiguration>();
            ISQLDocumentRepository _collectionRepository = DIProvider.GetServiceProvider().GetService<ISQLDocumentRepository>();
        }
    }
}
