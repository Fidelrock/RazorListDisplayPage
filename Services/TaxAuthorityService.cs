using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RazorTableDemo.Models;

namespace RazorTableDemo.Services
{
    public class TaxAuthorityService : BaseDataService, ITaxAuthorityService
    {
        public TaxAuthorityService(
            IConfiguration configuration, 
            ILogger<TaxAuthorityService> logger,
            IOptions<AppSettings> appSettings) 
            : base(configuration, logger, appSettings)
        {
        }

        public async Task<(IEnumerable<S300TaxAuthority> Results, int TotalCount, int TotalPages)> GetTaxAuthoritiesPaginatedAsync(
            string? authorityKey = null, int page = 1, int pageSize = 10)
        {
            try
            {
                _logger.LogDebug("Fetching tax authorities - Page: {Page}, PageSize: {PageSize}, AuthorityKey: {AuthorityKey}", 
                    page, pageSize, authorityKey ?? "null");
                
                // Validate parameters using base class method
                ValidatePaginationParameters(ref page, ref pageSize);
                
                // Build the WHERE clause using base class method
                var whereClause = GetClientCodeWhereClause();
                var parameters = new DynamicParameters();
                AddClientCodeParameter(parameters);
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
                var totalCount = await _connection.QuerySingleAsync<int>(countSql, parameters);

                // Calculate total pages
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

                // Get paginated results
                var sql = $@"
                    SELECT * FROM S300TaxAuthority 
                    {whereClause}
                    ORDER BY AuthorityKey
                    OFFSET @Offset ROWS 
                    FETCH NEXT @PageSize ROWS ONLY";

                var results = await _connection.QueryAsync<S300TaxAuthority>(sql, parameters);

                _logger.LogInformation("Successfully retrieved {Count} tax authorities (Page {Page} of {TotalPages})", 
                    results.Count(), page, totalPages);

                return (results, totalCount, totalPages);
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                _logger.LogError(ex, "Database error occurred while fetching tax authorities");
                throw new InvalidOperationException("Unable to retrieve tax authorities from database", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while fetching tax authorities");
                throw;
            }
        }
    }
} 