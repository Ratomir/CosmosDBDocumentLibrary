using Microsoft.Azure.Documents;
using Microsoft.Extensions.Configuration;
using AzureCosmosCore.Interface;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using AzureCosmosCore.ExceptionCore;

namespace AzureCosmosCore.Repository
{
    public class SQLStoredProcedureRepository : BaseDatabaseRepository, ISQLStoredProcedureRepository
    {
        public SQLStoredProcedureRepository(IConfiguration configuration) : base(configuration)
        {
        }

        #region << Run with StoredProcedure object >>

        public async Task<T> RunStoredProcedureAsync<T>(StoredProcedure sp, params dynamic[] parameters)
        {
            T result = await Client.ExecuteStoredProcedureAsync<T>(sp.SelfLink, parameters);
            return result;
        }

        public async Task<Document> RunStoredProcedureAsync(StoredProcedure sp, params dynamic[] parameters)
        {
            return await RunStoredProcedureAsync<Document>(sp, parameters);
        }

        #endregion << Run with StoredProcedure object >>

        #region << Run with Uri property >>
        public async Task<T> RunStoredProcedureAsync<T>(Uri sp, params dynamic[] parameters)
        {
            Document document = await Client.ExecuteStoredProcedureAsync<Document>(sp, parameters);

            ExceptionModel exception = document.GetPropertyValue<ExceptionModel>("exception");

            if (exception != null)
            {
                throw new Exception(exception.Message);
            }

            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(document));
        }

        public async Task<Document> RunStoredProcedureAsync(Uri sp, params dynamic[] parameters)
        {
            Document document = await Client.ExecuteStoredProcedureAsync<Document>(sp, parameters);
            ExceptionModel exception = document.GetPropertyValue<ExceptionModel>("exception");

            if (exception != null)
            {
                throw new Exception(exception.Message);
            }

            return document;
        }

        public async Task<StoredProcedure> GetStoredProcedureAsync(Uri sp)
        {
            return await Client.ReadStoredProcedureAsync(sp);
        }

        public async Task<StoredProcedure> CreateStoredProcedureAsync(Uri uri, StoredProcedure sp)
        {
            return await Client.CreateStoredProcedureAsync(uri, sp);
        }
        #endregion << Run with Uri property >>
    }
}
