using RazorTableDemo.Models;

namespace RazorTableDemo.Services
{
    public interface ITaxAuthorityService
    {
        Task<IEnumerable<S300TaxAuthority>> GetTaxAuthoritiesAsync(string? clientCode = null, string? authorityKey = null);
        Task<S300TaxAuthority?> GetTaxAuthorityByIdAsync(int id);
        Task<S300TaxAuthority> CreateTaxAuthorityAsync(S300TaxAuthority taxAuthority);
        Task<bool> UpdateTaxAuthorityAsync(S300TaxAuthority taxAuthority);
        Task<bool> DeleteTaxAuthorityAsync(int id);
    }
} 