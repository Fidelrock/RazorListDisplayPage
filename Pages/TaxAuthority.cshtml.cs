using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorTableDemo.Models;
using RazorTableDemo.Services;

public class TaxAuthorityModel : PageModel
{
    private readonly ITaxAuthorityService _taxAuthorityService;
    
    public TaxAuthorityModel(ITaxAuthorityService taxAuthorityService)
    {
        _taxAuthorityService = taxAuthorityService;
    }

    public string? SuccessMessage { get; set; }
    public string? ErrorMessage { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? ClientCode { get; set; }
    [BindProperty(SupportsGet = true)]
    public string? AuthorityKey { get; set; }
    
    [BindProperty(SupportsGet = true)]
    public new int Page { get; set; } = 1;
    
    [BindProperty(SupportsGet = true)]
    public int PageSize { get; set; } = 10;

    public List<S300TaxAuthority> Results { get; set; } = new List<S300TaxAuthority>();
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; }
    public bool HasPreviousPage => CurrentPage > 1;
    public bool HasNextPage => CurrentPage < TotalPages;

    public async Task OnGetAsync()
    {
        // Clear model state to remove any persisted error messages
        ModelState.Clear();
        
        // Clear any previous messages at the start
        ErrorMessage = null;
        SuccessMessage = null;
        
        try
        {
            // Always fetch data for pagination, even without search parameters
            var (results, totalCount, totalPages) = await _taxAuthorityService.GetTaxAuthoritiesPaginatedAsync(
                ClientCode, AuthorityKey, Page, PageSize);
            
            Results = results.ToList();
            TotalCount = totalCount;
            TotalPages = totalPages;
            CurrentPage = Page;
            
            if (Results.Count == 0)
            {
                if (!string.IsNullOrEmpty(ClientCode) || !string.IsNullOrEmpty(AuthorityKey))
                {
                    ErrorMessage = "No tax authorities found matching your criteria.";
                }
                else
                {
                    SuccessMessage = "No tax authorities found in the database.";
                }
            }
        }
        catch (Exception ex)
        {
            // Handle error appropriately
            ErrorMessage = $"Error loading data: {ex.Message}";
        }
    }
}