using Dapper;
using Microsoft.Data.SqlClient;
using RazorTableDemo.Models;

namespace RazorTableDemo.Services
{
    public class ICItemMapService : IICItemMapService
    {
        private readonly IConfiguration _configuration;

        public ICItemMapService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<(IEnumerable<ICItemMap> Results, int TotalCount, int TotalPages)> GetICItemMapsPaginatedAsync(
            string? clientCode = null, string? itemNumber = null, string? etimItemCode = null, string? itemClassCode = null, 
            int page = 1, int pageSize = 10)
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            
            // Build the WHERE clause
            var whereClause = "WHERE 1=1";
            var parameters = new DynamicParameters();
            parameters.Add("Page", page);
            parameters.Add("PageSize", pageSize);
            parameters.Add("Offset", (page - 1) * pageSize);

            if (!string.IsNullOrEmpty(clientCode))
            {
                whereClause += " AND ClientCode LIKE @ClientCode";
                parameters.Add("ClientCode", $"%{clientCode}%");
            }
            if (!string.IsNullOrEmpty(itemNumber))
            {
                whereClause += " AND ItemNumber LIKE @ItemNumber";
                parameters.Add("ItemNumber", $"%{itemNumber}%");
            }
            if (!string.IsNullOrEmpty(etimItemCode))
            {
                whereClause += " AND EtimItemCode LIKE @EtimItemCode";
                parameters.Add("EtimItemCode", $"%{etimItemCode}%");
            }
            if (!string.IsNullOrEmpty(itemClassCode))
            {
                whereClause += " AND ItemClassCode LIKE @ItemClassCode";
                parameters.Add("ItemClassCode", $"%{itemClassCode}%");
            }

            // Get total count
            var countSql = $"SELECT COUNT(*) FROM ICItemMap {whereClause}";
            var totalCount = await connection.QuerySingleAsync<int>(countSql, parameters);

            // Calculate total pages
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            // Get paginated results
            var sql = $@"
                SELECT * FROM ICItemMap 
                {whereClause}
                ORDER BY ClientCode, ItemNumber, ItemIndex
                OFFSET @Offset ROWS 
                FETCH NEXT @PageSize ROWS ONLY";

            var results = await connection.QueryAsync<ICItemMap>(sql, parameters);

            return (results, totalCount, totalPages);
        }
    }
} 