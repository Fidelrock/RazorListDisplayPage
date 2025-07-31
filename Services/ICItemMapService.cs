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
            string? itemNumber = null, string? etimItemCode = null, int page = 1, int pageSize = 10)
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            
            // Build the WHERE clause - hardcode CARLTD
            var whereClause = "WHERE ClientCode = 'CARLTD'";
            var parameters = new DynamicParameters();
            parameters.Add("Page", page);
            parameters.Add("PageSize", pageSize);
            parameters.Add("Offset", (page - 1) * pageSize);

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

            // Get total count
            var countSql = $"SELECT COUNT(*) FROM ICItemMap {whereClause}";
            var totalCount = await connection.QuerySingleAsync<int>(countSql, parameters);

            // Calculate total pages
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            // Get paginated results
            var sql = $@"
                SELECT * FROM ICItemMap 
                {whereClause}
                ORDER BY ItemNumber
                OFFSET @Offset ROWS 
                FETCH NEXT @PageSize ROWS ONLY";

            var results = await connection.QueryAsync<ICItemMap>(sql, parameters);

            return (results, totalCount, totalPages);
        }
    }
} 