using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RazorTableDemo.Models;

namespace RazorTableDemo.Services
{
    public class ICItemMapService : BaseDataService, IICItemMapService
    {
        public ICItemMapService(
            IConfiguration configuration, 
            ILogger<ICItemMapService> logger,
            IOptions<AppSettings> appSettings) 
            : base(configuration, logger, appSettings)
        {
        }

        public async Task<(IEnumerable<ICItemMap> Results, int TotalCount, int TotalPages)> GetICItemMapsPaginatedAsync(
            string? itemNumber = null, string? etimItemCode = null, int page = 1, int pageSize = 10)
        {
            try
            {
                _logger.LogDebug("Fetching IC item maps - Page: {Page}, PageSize: {PageSize}, ItemNumber: {ItemNumber}, EtimItemCode: {EtimItemCode}", 
                    page, pageSize, itemNumber ?? "null", etimItemCode ?? "null");
                
                // Validate parameters using base class method
                ValidatePaginationParameters(ref page, ref pageSize);
                
                // Build the WHERE clause using base class method
                var whereClause = GetClientCodeWhereClause();
                var parameters = new DynamicParameters();
                AddClientCodeParameter(parameters);
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
                var totalCount = await _connection.QuerySingleAsync<int>(countSql, parameters);

                // Calculate total pages
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

                // Get paginated results
                var sql = $@"
                    SELECT * FROM ICItemMap 
                    {whereClause}
                    ORDER BY ItemNumber
                    OFFSET @Offset ROWS 
                    FETCH NEXT @PageSize ROWS ONLY";

                var results = await _connection.QueryAsync<ICItemMap>(sql, parameters);

                _logger.LogInformation("Successfully retrieved {Count} IC item maps (Page {Page} of {TotalPages})", 
                    results.Count(), page, totalPages);

                return (results, totalCount, totalPages);
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                _logger.LogError(ex, "Database error occurred while fetching IC item maps");
                throw new InvalidOperationException("Unable to retrieve IC item maps from database", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while fetching IC item maps");
                throw;
            }
        }
    }
} 