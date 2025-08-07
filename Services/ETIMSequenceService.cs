using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RazorTableDemo.Models;

namespace RazorTableDemo.Services
{
    public class ETIMSequenceService : BaseDataService, IETIMSequenceService
    {
        public ETIMSequenceService(
            IConfiguration configuration, 
            ILogger<ETIMSequenceService> logger,
            IOptions<AppSettings> appSettings) 
            : base(configuration, logger, appSettings)
        {
        }

        public async Task<(IEnumerable<ETIMSequence> Results, int TotalCount, int TotalPages)> GetETIMSequencesPaginatedAsync(
            int page = 1, int pageSize = 10)
        {
            try
            {
                _logger.LogDebug("Fetching ETIM sequences - Page: {Page}, PageSize: {PageSize}", page, pageSize);
                
                // Validate parameters using base class method
                ValidatePaginationParameters(ref page, ref pageSize);
                
                // Build the WHERE clause using base class method
                var whereClause = GetClientCodeWhereClause();
                var parameters = new DynamicParameters();
                AddClientCodeParameter(parameters);
                parameters.Add("Page", page);
                parameters.Add("PageSize", pageSize);
                parameters.Add("Offset", (page - 1) * pageSize);

                // Get total count
                var countSql = $"SELECT COUNT(*) FROM ETIMSequence {whereClause}";
                var totalCount = await _connection.QuerySingleAsync<int>(countSql, parameters);

                // Calculate total pages
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

                // Get paginated results
                var sql = $@"
                    SELECT * FROM ETIMSequence 
                    {whereClause}
                    ORDER BY CreatedOn DESC
                    OFFSET @Offset ROWS 
                    FETCH NEXT @PageSize ROWS ONLY";

                var results = await _connection.QueryAsync<ETIMSequence>(sql, parameters);

                _logger.LogInformation("Successfully retrieved {Count} ETIM sequences (Page {Page} of {TotalPages})", 
                    results.Count(), page, totalPages);

                return (results, totalCount, totalPages);
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                _logger.LogError(ex, "Database error occurred while fetching ETIM sequences");
                throw new InvalidOperationException("Unable to retrieve ETIM sequences from database", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while fetching ETIM sequences");
                throw;
            }
        }
    }
} 