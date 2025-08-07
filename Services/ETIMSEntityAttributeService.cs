using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RazorTableDemo.Models;

namespace RazorTableDemo.Services
{
    public class ETIMSEntityAttributeService : BaseDataService, IETIMSEntityAttributeService
    {
        public ETIMSEntityAttributeService(
            IConfiguration configuration, 
            ILogger<ETIMSEntityAttributeService> logger,
            IOptions<AppSettings> appSettings) 
            : base(configuration, logger, appSettings)
        {
        }

        public async Task<(IEnumerable<ETIMSEntityAttribute> Results, int TotalCount, int TotalPages)> GetETIMSEntityAttributesPaginatedAsync(
            string? entityType = null, string? searchKey = null, int page = 1, int pageSize = 10)
        {
            try
            {
                _logger.LogDebug("Fetching ETIMS entity attributes - Page: {Page}, PageSize: {PageSize}, EntityType: {EntityType}, SearchKey: {SearchKey}", 
                    page, pageSize, entityType ?? "null", searchKey ?? "null");
                
                // Validate parameters using base class method
                ValidatePaginationParameters(ref page, ref pageSize);
                
                // Build the WHERE clause using base class method
                var whereClause = GetClientCodeWhereClause();
                var parameters = new DynamicParameters();
                AddClientCodeParameter(parameters);
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
                var totalCount = await _connection.QuerySingleAsync<int>(countSql, parameters);

                // Calculate total pages
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

                // Get paginated results
                var sql = $@"
                    SELECT * FROM ETIMSEntityAttribute 
                    {whereClause}
                    ORDER BY EntityType, SearchKey
                    OFFSET @Offset ROWS 
                    FETCH NEXT @PageSize ROWS ONLY";

                var results = await _connection.QueryAsync<ETIMSEntityAttribute>(sql, parameters);

                _logger.LogInformation("Successfully retrieved {Count} ETIMS entity attributes (Page {Page} of {TotalPages})", 
                    results.Count(), page, totalPages);

                return (results, totalCount, totalPages);
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                _logger.LogError(ex, "Database error occurred while fetching ETIMS entity attributes");
                throw new InvalidOperationException("Unable to retrieve ETIMS entity attributes from database", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while fetching ETIMS entity attributes");
                throw;
            }
        }
    }
} 