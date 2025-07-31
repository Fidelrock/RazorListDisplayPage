using Dapper;
using Microsoft.Data.SqlClient;
using RazorTableDemo.Models;

namespace RazorTableDemo.Services
{
    public class TaxAuthorityService : ITaxAuthorityService
    {
        private readonly IConfiguration _configuration;

        public TaxAuthorityService(IConfiguration configuration)
        {
            _configuration = configuration;
        }



        public async Task<(IEnumerable<S300TaxAuthority> Results, int TotalCount, int TotalPages)> GetTaxAuthoritiesPaginatedAsync(
            string? authorityKey = null, int page = 1, int pageSize = 10)
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            
            // Build the WHERE clause - hardcode CARLTD
            var whereClause = "WHERE ClientCode = 'CARLTD'";
            var parameters = new DynamicParameters();
            parameters.Add("Page", page);
            parameters.Add("PageSize", pageSize);
            parameters.Add("Offset", (page - 1) * pageSize);

            if (!string.IsNullOrEmpty(authorityKey))
            {
                whereClause += " AND AuthorityKey LIKE @AuthorityKey";
                parameters.Add("AuthorityKey", $"%{authorityKey}%");
            }

            // Get total count
            var countSql = $"SELECT COUNT(*) FROM S300TaxAuthority {whereClause}";
            var totalCount = await connection.QuerySingleAsync<int>(countSql, parameters);

            // Calculate total pages
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            // Get paginated results
            var sql = $@"
                SELECT * FROM S300TaxAuthority 
                {whereClause}
                ORDER BY AuthorityKey
                OFFSET @Offset ROWS 
                FETCH NEXT @PageSize ROWS ONLY";

            var results = await connection.QueryAsync<S300TaxAuthority>(sql, parameters);

            return (results, totalCount, totalPages);
        }


    }
} 