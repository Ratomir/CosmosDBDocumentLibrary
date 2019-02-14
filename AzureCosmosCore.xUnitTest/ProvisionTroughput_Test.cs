using AzureCosmos.DI_Container;
using AzureCosmosCore.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace AzureCosmosCore.xUnitTest
{

    public class ProvisionTroughput_Test
    {
        [Fact]
        public async Task ChangeProvisionTroughput()
        {
            DIProvider provider = new DIProvider("appsettings - Copy.json", Directory.GetCurrentDirectory());
            ISQLCollectionRepository _collectionRepository = DIProvider.GetServiceProvider().GetService<ISQLCollectionRepository>();

            Assert.True(await _collectionRepository.ChangeDatabaseTroughput(1000));
        }
    }
}
