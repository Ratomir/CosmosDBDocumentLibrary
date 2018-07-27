using Microsoft.Azure.Documents;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzureCosmosCore.Interface
{ 
    public interface ISQLDatabaseRepository : IBaseRepository
    {
        #region << Database >>
        
        Task CreateDatabaseAsync(string databaseName);
        Task<bool> CheckIfDatabaseExistAsync(string databaseName);
        Task<bool> DeleteDatabase(string databaseName);
        List<Database> ListDatabaseForAccount();

        #endregion << Database >>
    }
}
