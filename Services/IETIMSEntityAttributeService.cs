using RazorTableDemo.Models;

namespace RazorTableDemo.Services
{
    public interface IETIMSEntityAttributeService
    {
        Task<(IEnumerable<ETIMSEntityAttribute> Results, int TotalCount, int TotalPages)> GetETIMSEntityAttributesPaginatedAsync(
            string? entityType = null, 
            string? searchKey = null, 
            int page = 1, 
            int pageSize = 10);
    }
} 