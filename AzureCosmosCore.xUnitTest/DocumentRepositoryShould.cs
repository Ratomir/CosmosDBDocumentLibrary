using AzureCosmos.DI_Container;
using AzureCosmosCore.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Azure.Documents;
using AzureCosmosCore.Model;

namespace AzureCosmosCore.xUnitTest
{
    public class DocumentRepositoryShould
    {
        [Fact]
        public async Task DeleteDocumentByQuery()
        {
            DIProvider provider = new DIProvider("appsettings - Copy.json", Directory.GetCurrentDirectory());
            ISQLDocumentRepository _documentRepository = DIProvider.GetServiceProvider().GetService<ISQLDocumentRepository>();

            SqlQuerySpec query = new SqlQuerySpec()
            {
                QueryText = "SELECT * FROM root r WHERE r._ts < @date",
                Parameters = new SqlParameterCollection()
                {
                    new SqlParameter("@date", new DateTime(2019, 3, 1).ToUnixTime())
                }
            };

            try
            {
                await _documentRepository.DeleteDocumentByCondition("sn-db", "sn-coll", query);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
