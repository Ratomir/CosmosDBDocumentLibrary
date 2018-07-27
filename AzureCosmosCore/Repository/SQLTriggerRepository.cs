using Microsoft.Azure.Documents;
using Microsoft.Extensions.Configuration;
using AzureCosmosCore.Interface;
using System;
using System.Threading.Tasks;

namespace AzureCosmosCore.Repository
{
    public class SQLTriggerRepository : BaseDatabaseRepository, ISQLTriggerRepository
    {
        public SQLTriggerRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<Trigger> CreateTriggerAsync(Uri collection, Trigger trigger)
        {
            return await Client.CreateTriggerAsync(collection, trigger);
        }
    }
}
