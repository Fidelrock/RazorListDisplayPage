using RazorTableDemo.Models;

namespace RazorTableDemo.Services
{
    public interface ISalesInvoiceService
    {
        Task<(IEnumerable<SalesInvoice> Results, int TotalCount, int TotalPages)> GetSalesInvoicesPaginatedAsync(
            string? clientCode = null,
            string? docNumber = null,
            string? customerName = null,
            string? docType = null,
            DateTime? fromDate = null,
            DateTime? toDate = null,
            int page = 1,
            int pageSize = 10);
    }
} 