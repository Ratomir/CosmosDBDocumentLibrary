using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Threading.Tasks;

namespace AzureCosmosCore.Interface
{
    public interface ISQLUDFRepository : IBaseRepository
    {
        Task<UserDefinedFunction> CreateUDFAsync(Uri collection, UserDefinedFunction udf, RequestOptions requestOptions = null);
    }
}
