using Microsoft.Azure.Documents;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzureCosmosCore.Interface
{
    public interface ISQLCollectionRepository : IBaseRepository
    {
        Task<DocumentCollection> CreateCollection(string databaseName, string collectionId);
        Task<bool> DeleteCollection(string databaseName, string collectionId);
        List<DocumentCollection> ReadAllCollections(string databaseName);
        bool CheckIfCollectionExistAsync(string databaseName, string collectionId);
        DocumentCollection GetDocumentCollection(string databaseName, string collectionId);
    }
}
