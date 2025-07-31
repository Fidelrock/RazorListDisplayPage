using RazorTableDemo.Models;

namespace RazorTableDemo.Services
{
    public interface IICItemMapService
    {
        Task<(IEnumerable<ICItemMap> Results, int TotalCount, int TotalPages)> GetICItemMapsPaginatedAsync(
            string? itemNumber = null, 
            string? etimItemCode = null, 
            int page = 1, 
            int pageSize = 10);
    }
} 