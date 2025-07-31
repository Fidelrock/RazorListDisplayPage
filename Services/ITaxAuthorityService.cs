using RazorTableDemo.Models;

namespace RazorTableDemo.Services
{
    public interface ITaxAuthorityService
    {
        Task<(IEnumerable<S300TaxAuthority> Results, int TotalCount, int TotalPages)> GetTaxAuthoritiesPaginatedAsync(
            string? clientCode = null, 
            string? authorityKey = null, 
            int page = 1, 
            int pageSize = 10);
    }
} 