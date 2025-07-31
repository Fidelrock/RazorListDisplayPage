using Dapper;
using Microsoft.Data.SqlClient;
using RazorTableDemo.Models;

namespace RazorTableDemo.Services
{
    public class ETIMSEntityAttributeService : IETIMSEntityAttributeService
    {
        private readonly IConfiguration _configuration;

        public ETIMSEntityAttributeService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<(IEnumerable<ETIMSEntityAttribute> Results, int TotalCount, int TotalPages)> GetETIMSEntityAttributesPaginatedAsync(
            string? entityType = null, string? searchKey = null, int page = 1, int pageSize = 10)
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            
            // Build the WHERE clause - hardcode CARLTD
            var whereClause = "WHERE ClientCode = 'CARLTD'";
            var parameters = new DynamicParameters();
            parameters.Add("Page", page);
            parameters.Add("PageSize", pageSize);
            parameters.Add("Offset", (page - 1) * pageSize);

            if (!string.IsNullOrEmpty(entityType))
            {
                whereClause += " AND EntityType LIKE @EntityType";
                parameters.Add("EntityType", $"%{entityType}%");
            }
            if (!string.IsNullOrEmpty(searchKey))
            {
                whereClause += " AND SearchKey LIKE @SearchKey";
                parameters.Add("SearchKey", $"%{searchKey}%");
            }

            // Get total count
            var countSql = $"SELECT COUNT(*) FROM ETIMSEntityAttribute {whereClause}";
            var totalCount = await connection.QuerySingleAsync<int>(countSql, parameters);

            // Calculate total pages
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            // Get paginated results
            var sql = $@"
                SELECT * FROM ETIMSEntityAttribute 
                {whereClause}
                ORDER BY EntityType, SearchKey
                OFFSET @Offset ROWS 
                FETCH NEXT @PageSize ROWS ONLY";

            var results = await connection.QueryAsync<ETIMSEntityAttribute>(sql, parameters);

            return (results, totalCount, totalPages);
        }
    }
} 