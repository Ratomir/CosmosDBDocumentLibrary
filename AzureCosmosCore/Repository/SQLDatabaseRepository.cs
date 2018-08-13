using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using AzureCosmosCore.Interface;
using Microsoft.Extensions.Configuration;
using AzureCosmosCore.Model;

namespace AzureCosmosCore.Repository
{
    public class SQLDatabaseRepository : BaseDatabaseRepository, ISQLDatabaseRepository
    {
        public SQLDatabaseRepository(IConfiguration configuration, ExternalMasterKeyEndpointUrlModel externalMasterKeyEndpointUrl = null) : base(configuration, externalMasterKeyEndpointUrl)
        {
        }

        public async Task<bool> CheckIfDatabaseExistAsync(string databaseName)
        {
            return await Task.Run(() => {

                Database database = Client.CreateDatabaseQuery().Where(t => t.Id == databaseName).ToList().FirstOrDefault(t => t.Id == databaseName);

                return database != null;
            });
        }

        public async Task CreateDatabaseAsync(string databaseName)
        {
            Database newDatabase = new Database
            {
                Id = databaseName
            };

            RequestOptions newRequestOptions = new RequestOptions()
            {
                OfferThroughput = 400
            };

            await Client.CreateDatabaseIfNotExistsAsync(newDatabase, newRequestOptions);
        }

        public async Task<bool> DeleteDatabase(string databaseName)
        {
            Database database = GetDatabaseQuery(databaseName);

            ResourceResponse<Database> response = await Client.DeleteDatabaseAsync(database.SelfLink);
            return await CheckIfDatabaseExistAsync(databaseName);
        }

        public List<Database> ListDatabaseForAccount() => Client.CreateDatabaseQuery().ToList();
    }
}
