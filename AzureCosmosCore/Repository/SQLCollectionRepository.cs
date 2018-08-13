using AzureCosmosCore.Interface;
using AzureCosmosCore.Model;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureCosmosCore.Repository
{
    public class SQLCollectionRepository : BaseDatabaseRepository, ISQLCollectionRepository
    {
        public SQLCollectionRepository(IConfiguration configuration, ExternalMasterKeyEndpointUrlModel externalMasterKeyEndpointUrl = null) : base(configuration, externalMasterKeyEndpointUrl)
        {
        }

        public bool CheckIfCollectionExistAsync(string databaseName, string collectionId) => GetDocumentCollection(databaseName, collectionId) != null;

        public async Task<DocumentCollection> CreateCollection(string databaseName, string collectionId, RequestOptions requestOptions = null)
        {
            Database database = GetDatabaseQuery(databaseName);

            DocumentCollection collection = new DocumentCollection()
            {
                Id = collectionId
            };

            DocumentCollection documentCollection = await Client.CreateDocumentCollectionIfNotExistsAsync(database.SelfLink, collection, requestOptions );

            return documentCollection;
        }

        public async Task<bool> DeleteCollection(string databaseName, string collectionId)
        {
            DocumentCollection collection = await Client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(databaseName, collectionId));

            await Client.DeleteDocumentCollectionAsync(collection.SelfLink);
            bool isCollectionExist = CheckIfCollectionExistAsync(databaseName, collectionId);
            return !isCollectionExist; //collection don't exist, ok, we got false but we need inverse value because delete process
        }

        public async Task<bool> DropCollectionDocument(string databaseName, string collectionId)
        {
            SqlQuerySpec query = new SqlQuerySpec()
            {
                QueryText = "SELECT * FROM c"
            };

            IDocumentQuery<Document> documents = Client.CreateDocumentQuery<Document>(UriFactory.CreateDocumentCollectionUri(databaseName, collectionId), query).AsDocumentQuery();

            while (documents.HasMoreResults)
            {
                foreach (Document doc in await documents.ExecuteNextAsync())
                {
                    await Client.DeleteDocumentAsync(doc.SelfLink);
                }
            }

            return Client.CreateDocumentQuery<Document>(UriFactory.CreateDocumentCollectionUri(databaseName, collectionId), query).AsEnumerable().Any();
        }

        public DocumentCollection GetDocumentCollection(string databaseName, string collectionId)
        {
            Database database = GetDatabaseQuery(databaseName);

            DocumentCollection documentCollection = Client.CreateDocumentCollectionQuery(database.CollectionsLink).Where(t => t.Id == collectionId).ToList().FirstOrDefault();

            return documentCollection;
        }

        public List<DocumentCollection> ReadAllCollections(string databaseName)
        {
            Database database = GetDatabaseQuery(databaseName);

            return Client.CreateDocumentCollectionQuery(database.CollectionsLink).ToList();
        }
    }
}
