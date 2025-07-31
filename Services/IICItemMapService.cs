using RazorTableDemo.Models;

namespace RazorTableDemo.Services
{
    public interface IICItemMapService
    {
        Task<(IEnumerable<ICItemMap> Results, int TotalCount, int TotalPages)> GetICItemMapsPaginatedAsync(
            string? clientCode = null,
            string? itemNumber = null,
            string? etimItemCode = null,
            string? itemClassCode = null,
            int page = 1,
            int pageSize = 10);
    }
} 