using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;
using AzureCosmosCore.Interface;
using System;
using System.Threading.Tasks;
using AzureCosmosCore.Model;

namespace AzureCosmosCore.Repository
{
    public class SQLUDFRepository : BaseDatabaseRepository, ISQLUDFRepository
    {
        public SQLUDFRepository(IConfiguration configuration, ExternalMasterKeyEndpointUrlModel externalMasterKeyEndpointUrl = null) : base(configuration, externalMasterKeyEndpointUrl)
        {
        }

        public async Task<UserDefinedFunction> CreateUDFAsync(Uri collection, UserDefinedFunction udf, RequestOptions requestOptions = null)
        {
            return await Client.CreateUserDefinedFunctionAsync(collection, udf, requestOptions); 
        }
    }
}
