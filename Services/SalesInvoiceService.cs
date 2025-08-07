using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RazorTableDemo.Models;

namespace RazorTableDemo.Services
{
    public class SalesInvoiceService : BaseDataService, ISalesInvoiceService
    {
        public SalesInvoiceService(
            IConfiguration configuration, 
            ILogger<SalesInvoiceService> logger,
            IOptions<AppSettings> appSettings) 
            : base(configuration, logger, appSettings)
        {
        }

        public async Task<(IEnumerable<SalesInvoice> Results, int TotalCount, int TotalPages)> GetSalesInvoicesPaginatedAsync(
            string? invoiceNumber = null, DateTime? fromDate = null, DateTime? toDate = null, int page = 1, int pageSize = 10)
        {
            try
            {
                _logger.LogDebug("Fetching sales invoices - Page: {Page}, PageSize: {PageSize}, InvoiceNumber: {InvoiceNumber}, FromDate: {FromDate}, ToDate: {ToDate}", 
                    page, pageSize, invoiceNumber ?? "null", fromDate?.ToString("yyyy-MM-dd") ?? "null", toDate?.ToString("yyyy-MM-dd") ?? "null");
                
                // Validate parameters using base class method
                ValidatePaginationParameters(ref page, ref pageSize);
                
                // Build the WHERE clause using base class method
                var whereClause = GetClientCodeWhereClause();
                var parameters = new DynamicParameters();
                AddClientCodeParameter(parameters);
                parameters.Add("Page", page);
                parameters.Add("PageSize", pageSize);
                parameters.Add("Offset", (page - 1) * pageSize);

                if (!string.IsNullOrEmpty(invoiceNumber))
                {
                    whereClause += " AND DocNumber LIKE @InvoiceNumber";
                    parameters.Add("InvoiceNumber", $"%{invoiceNumber}%");
                }
                if (fromDate.HasValue)
                {
                    whereClause += " AND DocDate >= @FromDate";
                    parameters.Add("FromDate", fromDate.Value.Date);
                }
                if (toDate.HasValue)
                {
                    whereClause += " AND DocDate <= @ToDate";
                    parameters.Add("ToDate", toDate.Value.Date);
                }

                // Get total count
                var countSql = $"SELECT COUNT(*) FROM SalesInvoice {whereClause}";
                var totalCount = await _connection.QuerySingleAsync<int>(countSql, parameters);

                // Calculate total pages
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

                // Get paginated results
                var sql = $@"
                    SELECT * FROM SalesInvoice 
                    {whereClause}
                    ORDER BY DocDate DESC, DocNumber
                    OFFSET @Offset ROWS 
                    FETCH NEXT @PageSize ROWS ONLY";

                var results = await _connection.QueryAsync<SalesInvoice>(sql, parameters);

                _logger.LogInformation("Successfully retrieved {Count} sales invoices (Page {Page} of {TotalPages})", 
                    results.Count(), page, totalPages);

                return (results, totalCount, totalPages);
            }
            catch (Microsoft.Data.SqlClient.SqlException ex)
            {
                _logger.LogError(ex, "Database error occurred while fetching sales invoices");
                throw new InvalidOperationException("Unable to retrieve sales invoices from database", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while fetching sales invoices");
                throw;
            }
        }
    }
} 