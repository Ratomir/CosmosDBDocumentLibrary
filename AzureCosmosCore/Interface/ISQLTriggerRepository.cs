using Microsoft.Azure.Documents;
using System;
using System.Threading.Tasks;

namespace AzureCosmosCore.Interface
{
    public interface ISQLTriggerRepository : IBaseRepository
    {
        Task<Trigger> CreateTriggerAsync(Uri collection, Trigger trigger);
    }
}
