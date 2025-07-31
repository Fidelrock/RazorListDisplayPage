using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorTableDemo.Models;
using RazorTableDemo.Services;

namespace RazorTableDemo.Pages
{
    public class ICItemMapModel : PageModel
    {
        private readonly IICItemMapService _icItemMapService;
        
        public ICItemMapModel(IICItemMapService icItemMapService)
        {
            _icItemMapService = icItemMapService;
        }

        public string? SuccessMessage { get; set; }
        public string? ErrorMessage { get; set; }

        // Search parameters
        [BindProperty(SupportsGet = true)]
        public string? ItemNumber { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public string? EtimItemCode { get; set; }
        
        // Pagination properties
        [BindProperty(SupportsGet = true)]
        public new int Page { get; set; } = 1;
        
        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 10;

        public List<ICItemMap> Results { get; set; } = new List<ICItemMap>();
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
                var (results, totalCount, totalPages) = await _icItemMapService.GetICItemMapsPaginatedAsync(
                    ItemNumber, EtimItemCode, Page, PageSize);
                
                Results = results.ToList();
                TotalCount = totalCount;
                TotalPages = totalPages;
                CurrentPage = Page;
                
                if (Results.Count == 0)
                {
                    if (!string.IsNullOrEmpty(ItemNumber) || !string.IsNullOrEmpty(EtimItemCode))
                    {
                        ErrorMessage = "No item mappings found matching your criteria.";
                    }
                    else
                    {
                        SuccessMessage = "No item mappings found in the database.";
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
}
