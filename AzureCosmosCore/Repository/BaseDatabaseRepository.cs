using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using AzureCosmosCore.Interface;
using System;
using System.Linq;
using AzureCosmosCore.Model;

namespace AzureCosmosCore.Repository
{
    public class BaseDatabaseRepository : IBaseRepository
    {
        public DocumentClient Client { get; set; }
        private FeedOptions defaultOptions = new FeedOptions { EnableCrossPartitionQuery = true };
        public string EndPoint { get; set; }
        public string MasterKey { get; set; }

        public Uri UriCosmosDB { get; set; }
        public FeedOptions DefaultOptions { get => defaultOptions; set => defaultOptions = value; }

        public BaseDatabaseRepository(IConfiguration configruation, ExternalMasterKeyEndpointUrlModel externalMasterKeyEndpointUrl = null)
        {
            string endpointUrl = "endpointUrl";
            string masterKey = "masterKey";

            if (externalMasterKeyEndpointUrl != null)
            {
                endpointUrl = externalMasterKeyEndpointUrl.ExternalEndpointUrlPath;
                masterKey = externalMasterKeyEndpointUrl.ExternalMasterKeyPath;
            }

            EndPoint = configruation.GetSection(endpointUrl).Value;
            MasterKey = configruation.GetSection(masterKey).Value;

            UriCosmosDB = new Uri(EndPoint);
            ConnectionPolicy connectionPolicy = new ConnectionPolicy();
            connectionPolicy.PreferredLocations.Add(LocationNames.WestUS);
            connectionPolicy.PreferredLocations.Add(LocationNames.NorthEurope);
            connectionPolicy.PreferredLocations.Add(LocationNames.FranceCentral);

            Client = new DocumentClient(UriCosmosDB, MasterKey, connectionPolicy, ConsistencyLevel.Session);
        }

        public Database GetDatabaseQuery(string databaseName)
        {
            SqlQuerySpec query = new SqlQuerySpec()
            {
                QueryText = "SELECT * FROM c WHERE c.id = @id",
                Parameters = new SqlParameterCollection()
                {
                    new SqlParameter("@id", databaseName)
                }
            };

            return Client.CreateDatabaseQuery(query).AsEnumerable().First();
        }
    }
}
