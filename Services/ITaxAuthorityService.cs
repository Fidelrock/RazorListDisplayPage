using RazorTableDemo.Models;

namespace RazorTableDemo.Services
{
    public interface ITaxAuthorityService
    {
        Task<IEnumerable<S300TaxAuthority>> GetTaxAuthoritiesAsync(string? clientCode = null, string? authorityKey = null);
        Task<(IEnumerable<S300TaxAuthority> Results, int TotalCount, int TotalPages)> GetTaxAuthoritiesPaginatedAsync(
            string? clientCode = null, 
            string? authorityKey = null, 
            int page = 1, 
            int pageSize = 10);
        Task<S300TaxAuthority?> GetTaxAuthorityByIdAsync(int id);
        Task<S300TaxAuthority> CreateTaxAuthorityAsync(S300TaxAuthority taxAuthority);
        Task<bool> UpdateTaxAuthorityAsync(S300TaxAuthority taxAuthority);
        Task<bool> DeleteTaxAuthorityAsync(int id);
    }
} 