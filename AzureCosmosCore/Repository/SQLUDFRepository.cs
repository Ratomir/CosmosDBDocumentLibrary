using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using AzureCosmosCore.Interface;
using System;
using System.Threading.Tasks;

namespace AzureCosmosCore.Repository
{
    public class SQLUDFRepository : BaseDatabaseRepository, ISQLUDFRepository
    {
        public SQLUDFRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<UserDefinedFunction> CreateUDFAsync(Uri collection, UserDefinedFunction udf, RequestOptions requestOptions = null)
        {
            return await Client.CreateUserDefinedFunctionAsync(collection, udf, requestOptions); 
        }
    }
}
