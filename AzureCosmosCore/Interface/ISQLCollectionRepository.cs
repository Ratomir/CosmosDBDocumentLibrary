using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzureCosmosCore.Interface
{
    public interface ISQLCollectionRepository : IBaseRepository
    {
        Task<DocumentCollection> CreateCollectionIfNotExistsAsync(string databaseName, string collectionId, RequestOptions requestOptions = null);
        Task<DocumentCollection> CreateCollectionAsync(string databaseName, string collectionId, RequestOptions requestOptions = null);
        Task<bool> DeleteCollection(string databaseName, string collectionId);
        List<DocumentCollection> ReadAllCollections(string databaseName);
        bool CheckIfCollectionExistAsync(string databaseName, string collectionId);
        DocumentCollection GetDocumentCollection(string databaseName, string collectionId);
        Task<bool> DropCollectionDocument(string databaseName, string collectionId);
    }
}
