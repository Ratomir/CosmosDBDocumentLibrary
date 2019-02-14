using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using AzureCosmosCore.Interface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureCosmosCore.Model;
using System.Net;

namespace AzureCosmosCore.Repository
{
    public class SQLDocumentRepository : BaseDatabaseRepository, ISQLDocumentRepository
    {
        public SQLDocumentRepository(IConfiguration configuration, ExternalMasterKeyEndpointUrlModel externalMasterKeyEndpointUrl = null) : base(configuration, externalMasterKeyEndpointUrl)
        {
        }

        public async Task<bool> DeleteDocument(string databaseId, string collectionId, string id)
        {
            ResourceResponse<Document> response = await Client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(databaseId, collectionId, id));
            return response.StatusCode == HttpStatusCode.OK;
        }

        public async Task<bool> DeleteDocument(Uri documentLink)
        {
            ResourceResponse<Document> response = await Client.DeleteDocumentAsync(documentLink);
            return response.StatusCode == HttpStatusCode.OK;
        }

        public async Task<Document> InsertDocument(string databaseId, string collectionId, object document, RequestOptions requestOptions = null)
        {
            Document documentResponse = await Client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(databaseId, collectionId), document, requestOptions);
            return documentResponse;
        }

        public async Task<Document> InsertDocument(Uri collectionLink, object document, RequestOptions requestOptions = null)
        {
            Document documentResponse = await Client.CreateDocumentAsync(collectionLink, document, requestOptions);
            return documentResponse;
        }

        public async Task<T> ReadDocumentByIdAsync<T>(object id, Uri collectionUri)
        {
            SqlQuerySpec query = new SqlQuerySpec()
            {
                QueryText = "SELECT * FROM root r WHERE r.id = @id",
                Parameters = new SqlParameterCollection()
                {
                    new SqlParameter("@id", id)
                }
            };

            T document = await Task.Run(() => Client.CreateDocumentQuery<T>(collectionUri, query, DefaultOptions).ToList().FirstOrDefault());
            return document;
        }

        public IQueryable<T> ReadDocumentByQuery<T>(Uri collectionUri, SqlQuerySpec query)
        {
            return Client.CreateDocumentQuery<T>(collectionUri, query, DefaultOptions);
        }

        public async Task<T> UpdateDocument<T>(Uri documentLink, object document, RequestOptions requestOptions = null) where T:class
        {
            Document result = await Client.ReplaceDocumentAsync(documentLink, document, requestOptions);
            if (typeof(T) != typeof(Document))
            {
                return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(result));
            }

            return result as T;
        }

        public async Task<List<T>> ReadAllDocuments<T>(Uri collectionUri)
        {
            SqlQuerySpec query = new SqlQuerySpec("SELECT * FROM root");

            List<T> document = await Task.Run(() => Client.CreateDocumentQuery<T>(collectionUri, query, DefaultOptions).ToList());
            return document;
        }

        public async Task<List<T>> ReadAllDocuments<T>(Uri collectionUri, SqlQuerySpec query)
        {
            List<T> document = await Task.Run(() => Client.CreateDocumentQuery<T>(collectionUri, query, DefaultOptions).ToList());
            return document;
        }

        public async Task DeleteDocumentByCondition(string databaseId, string collectionId, SqlQuerySpec query)
        {
            List<Document> lstDocuments = await ReadAllDocuments<Document>(UriFactory.CreateDocumentCollectionUri(databaseId, collectionId), query);

            ParallelLoopResult result = Parallel.ForEach(lstDocuments, async (document) => {
                bool deleted = await DeleteDocument(databaseId, collectionId, document.Id);
            });

            while (!result.IsCompleted) ;
        }
    }
}
