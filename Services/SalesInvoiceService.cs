using Dapper;
using Microsoft.Data.SqlClient;
using RazorTableDemo.Models;

namespace RazorTableDemo.Services
{
    public class SalesInvoiceService : ISalesInvoiceService
    {
        private readonly IConfiguration _configuration;

        public SalesInvoiceService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<(IEnumerable<SalesInvoice> Results, int TotalCount, int TotalPages)> GetSalesInvoicesPaginatedAsync(
            string? clientCode = null, string? docNumber = null, string? customerName = null, string? docType = null,
            DateTime? fromDate = null, DateTime? toDate = null, int page = 1, int pageSize = 10)
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
            if (!string.IsNullOrEmpty(docNumber))
            {
                whereClause += " AND DocNumber LIKE @DocNumber";
                parameters.Add("DocNumber", $"%{docNumber}%");
            }
            if (!string.IsNullOrEmpty(customerName))
            {
                whereClause += " AND CustomerName LIKE @CustomerName";
                parameters.Add("CustomerName", $"%{customerName}%");
            }
            if (!string.IsNullOrEmpty(docType))
            {
                whereClause += " AND DocType LIKE @DocType";
                parameters.Add("DocType", $"%{docType}%");
            }
            if (fromDate.HasValue)
            {
                whereClause += " AND DocDate >= @FromDate";
                parameters.Add("FromDate", fromDate.Value.Date);
            }
            if (toDate.HasValue)
            {
                whereClause += " AND DocDate <= @ToDate";
                parameters.Add("ToDate", toDate.Value.Date.AddDays(1).AddSeconds(-1));
            }

            // Get total count
            var countSql = $"SELECT COUNT(*) FROM SalesInvoice {whereClause}";
            var totalCount = await connection.QuerySingleAsync<int>(countSql, parameters);

            // Calculate total pages
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            // Get paginated results
            var sql = $@"
                SELECT * FROM SalesInvoice 
                {whereClause}
                ORDER BY DocDate DESC, ETRTrxID DESC
                OFFSET @Offset ROWS 
                FETCH NEXT @PageSize ROWS ONLY";

            var results = await connection.QueryAsync<SalesInvoice>(sql, parameters);

            return (results, totalCount, totalPages);
        }
    }
} 