using Dapper;
using Microsoft.Data.SqlClient;
using RazorTableDemo.Models;

namespace RazorTableDemo.Services
{
    public class ETIMSequenceService : IETIMSequenceService
    {
        private readonly IConfiguration _configuration;

        public ETIMSequenceService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<(IEnumerable<ETIMSequence> Results, int TotalCount, int TotalPages)> GetETIMSequencesPaginatedAsync(
            string? clientCode = null, int page = 1, int pageSize = 10)
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

            // Get total count
            var countSql = $"SELECT COUNT(*) FROM ETIMSequence {whereClause}";
            var totalCount = await connection.QuerySingleAsync<int>(countSql, parameters);

            // Calculate total pages
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            // Get paginated results
            var sql = $@"
                SELECT * FROM ETIMSequence 
                {whereClause}
                ORDER BY ClientCode
                OFFSET @Offset ROWS 
                FETCH NEXT @PageSize ROWS ONLY";

            var results = await connection.QueryAsync<ETIMSequence>(sql, parameters);

            return (results, totalCount, totalPages);
        }
    }
} 