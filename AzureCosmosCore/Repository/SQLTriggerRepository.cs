using Microsoft.Azure.Documents;
using Microsoft.Extensions.Configuration;
using AzureCosmosCore.Interface;
using System;
using System.Threading.Tasks;
using AzureCosmosCore.Model;

namespace AzureCosmosCore.Repository
{
    public class SQLTriggerRepository : BaseDatabaseRepository, ISQLTriggerRepository
    {
        public SQLTriggerRepository(IConfiguration configuration, ExternalMasterKeyEndpointUrlModel externalMasterKeyEndpointUrl = null) : base(configuration, externalMasterKeyEndpointUrl)
        {
        }

        public async Task<Trigger> CreateTriggerAsync(Uri collection, Trigger trigger)
        {
            return await Client.CreateTriggerAsync(collection, trigger);
        }
    }
}
