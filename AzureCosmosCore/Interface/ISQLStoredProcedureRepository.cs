using Microsoft.Azure.Documents;
using System;
using System.Threading.Tasks;

namespace AzureCosmosCore.Interface
{
    public interface ISQLStoredProcedureRepository : IBaseRepository
    {
        Task<T> RunStoredProcedureAsync<T>(StoredProcedure sp, params dynamic[] parameters);
        Task<Document> RunStoredProcedureAsync(StoredProcedure sp, params dynamic[] parameters);
        Task<T> RunStoredProcedureAsync<T>(Uri sp, params dynamic[] parameters);
        Task<Document> RunStoredProcedureAsync(Uri sp, params dynamic[] parameters);
        Task<StoredProcedure> GetStoredProcedureAsync(Uri sp);
        Task<StoredProcedure> CreateStoredProcedureAsync(Uri uri, StoredProcedure sp);
    }
}
