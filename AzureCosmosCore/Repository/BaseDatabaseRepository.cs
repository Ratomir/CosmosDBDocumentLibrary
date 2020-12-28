using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using AzureCosmosCore.Interface;
using System;
using System.Linq;
using AzureCosmosCore.Model;
using System.Security;
using System.Net;
using Microsoft.Azure.Cosmos;

namespace AzureCosmosCore.Repository
{
    public class BaseDatabaseRepository : IBaseRepository
    {
        public DocumentClient Client { get; set; }
        public string EndPoint { get; set; }
        public string MasterKey { get; set; }

        public Uri UriCosmosDB { get; set; }
        public FeedOptions DefaultOptions { get; set; } = new FeedOptions { EnableCrossPartitionQuery = true };

        public CosmosClient CosmosClient { get; set; }

        public BaseDatabaseRepository(IConfiguration configuration, ExternalMasterKeyEndpointUrlModel externalMasterKeyEndpointUrl = null)
        {
            string endpointUrl = "endpointUrl";
            string masterKey = "masterKey";

            if (externalMasterKeyEndpointUrl != null)
            {
                endpointUrl = externalMasterKeyEndpointUrl.ExternalEndpointUrlPath;
                masterKey = externalMasterKeyEndpointUrl.ExternalMasterKeyPath;
            }

            EndPoint = configuration.GetSection(endpointUrl).Value;
            MasterKey = configuration.GetSection(masterKey).Value;

            UriCosmosDB = new Uri(EndPoint);
            ConnectionPolicy connectionPolicy = new ConnectionPolicy();
            connectionPolicy.PreferredLocations.Add(LocationNames.WestUS);
            connectionPolicy.PreferredLocations.Add(LocationNames.NorthEurope);
            connectionPolicy.PreferredLocations.Add(LocationNames.FranceCentral);


            SecureString theSecureString = new NetworkCredential("", MasterKey).SecurePassword;
            Client = new DocumentClient(UriCosmosDB, theSecureString, connectionPolicy, Microsoft.Azure.Documents.ConsistencyLevel.Session);
            CosmosClient = new CosmosClient(EndPoint, MasterKey);
        }

        public Microsoft.Azure.Documents.Database GetDatabaseQuery(string databaseName)
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
