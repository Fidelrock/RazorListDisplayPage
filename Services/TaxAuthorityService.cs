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

        public async Task<IEnumerable<S300TaxAuthority>> GetTaxAuthoritiesAsync(string? clientCode = null, string? authorityKey = null)
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            var sql = "SELECT * FROM S300TaxAuthority WHERE 1=1";
            var parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(clientCode))
            {
                sql += " AND ClientCode LIKE @ClientCode";
                parameters.Add("ClientCode", $"%{clientCode}%");
            }
            if (!string.IsNullOrEmpty(authorityKey))
            {
                sql += " AND AuthorityKey LIKE @AuthorityKey";
                parameters.Add("AuthorityKey", $"%{authorityKey}%");
            }

            return await connection.QueryAsync<S300TaxAuthority>(sql, parameters);
        }

        public async Task<(IEnumerable<S300TaxAuthority> Results, int TotalCount, int TotalPages)> GetTaxAuthoritiesPaginatedAsync(
            string? clientCode = null, string? authorityKey = null, int page = 1, int pageSize = 10)
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
                ORDER BY ClientCode, AuthorityKey
                OFFSET @Offset ROWS 
                FETCH NEXT @PageSize ROWS ONLY";

            var results = await connection.QueryAsync<S300TaxAuthority>(sql, parameters);

            return (results, totalCount, totalPages);
        }

        public async Task<S300TaxAuthority?> GetTaxAuthorityByIdAsync(int id)
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            var sql = "SELECT * FROM S300TaxAuthority WHERE Id = @Id";
            return await connection.QueryFirstOrDefaultAsync<S300TaxAuthority>(sql, new { Id = id });
        }

        public async Task<S300TaxAuthority> CreateTaxAuthorityAsync(S300TaxAuthority taxAuthority)
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            var sql = @"INSERT INTO S300TaxAuthority (ClientCode, AuthorityKey, AuthorityName, CreatedOn) 
                       VALUES (@ClientCode, @AuthorityKey, @AuthorityName, @CreatedOn);
                       SELECT CAST(SCOPE_IDENTITY() as int)";
            
            var id = await connection.QuerySingleAsync<int>(sql, taxAuthority);
            taxAuthority.Id = id;
            
            return taxAuthority;
        }

        public async Task<bool> UpdateTaxAuthorityAsync(S300TaxAuthority taxAuthority)
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            var sql = @"UPDATE S300TaxAuthority 
                       SET ClientCode = @ClientCode, AuthorityKey = @AuthorityKey, 
                           AuthorityName = @AuthorityName, UpdatedOn = @UpdatedOn
                       WHERE Id = @Id";
            
            var rowsAffected = await connection.ExecuteAsync(sql, taxAuthority);
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteTaxAuthorityAsync(int id)
        {
            using var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            var sql = "DELETE FROM S300TaxAuthority WHERE Id = @Id";
            var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id });
            return rowsAffected > 0;
        }
    }
} 