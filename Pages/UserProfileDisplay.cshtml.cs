using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorTableDemo.Models;
using RazorTableDemo.Services;

public class UserProfileDisplayModel : PageModel
{
    private readonly ITaxAuthorityService _taxAuthorityService;
    
    public UserProfileDisplayModel(ITaxAuthorityService taxAuthorityService)
    {
        _taxAuthorityService = taxAuthorityService;
    }

    public string? SuccessMessage { get; set; }
    public string? ErrorMessage { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? ClientCode { get; set; }
    [BindProperty(SupportsGet = true)]
    public string? AuthorityKey { get; set; }

    public List<S300TaxAuthority> Results { get; set; } = new List<S300TaxAuthority>();

    public async Task OnGetAsync()
    {
        // Clear model state to remove any persisted error messages
        ModelState.Clear();
        
        // Clear any previous messages at the start
        ErrorMessage = null;
        SuccessMessage = null;
        
        try
        {
            // Only fetch data if search parameters are provided
            if (!string.IsNullOrEmpty(ClientCode) || !string.IsNullOrEmpty(AuthorityKey))
            {
                var results = await _taxAuthorityService.GetTaxAuthoritiesAsync(ClientCode, AuthorityKey);
                Results = results.ToList();
                
                if (Results.Count == 0)
                {
                    SuccessMessage = "No tax authorities found matching your criteria.";
                }
            }
            // If no search parameters, leave Results as empty list
        }
        catch (Exception ex)
        {
            // Handle error appropriately
            ErrorMessage = $"Error loading data: {ex.Message}";
        }
    }
}