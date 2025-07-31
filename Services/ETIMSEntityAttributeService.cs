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
            string? clientCode = null, string? entityType = null, string? searchKey = null, 
            string? entityKey = null, string? title = null, int page = 1, int pageSize = 10)
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
            if (!string.IsNullOrEmpty(entityKey))
            {
                whereClause += " AND EntityKey LIKE @EntityKey";
                parameters.Add("EntityKey", $"%{entityKey}%");
            }
            if (!string.IsNullOrEmpty(title))
            {
                whereClause += " AND Title LIKE @Title";
                parameters.Add("Title", $"%{title}%");
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
                ORDER BY SortOrder, ClientCode, EntityType, SearchKey
                OFFSET @Offset ROWS 
                FETCH NEXT @PageSize ROWS ONLY";

            var results = await connection.QueryAsync<ETIMSEntityAttribute>(sql, parameters);

            return (results, totalCount, totalPages);
        }
    }
} 