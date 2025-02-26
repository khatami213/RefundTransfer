using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;

namespace RefundTransferService.Helper_Code
{
    public class DapperHelper
    {
        //public async Task<IReadOnlyList<BalanceSheet>> GetByCompanyRefAsync(int companyRef)
        //{
        //    const string sql = "SELECT * FROM BalanceSheet WHERE CompanyRef = @companyRef";
        //    await using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        //    connection.Open();
        //    var result = await connection.QueryAsync<BalanceSheet>(sql, new { CompanyRef = companyRef });
        //    return result.ToList();
        //}
    }
}
