using RazorTableDemo.Models;

namespace RazorTableDemo.Services
{
    public interface IETIMSEntityAttributeService
    {
        Task<(IEnumerable<ETIMSEntityAttribute> Results, int TotalCount, int TotalPages)> GetETIMSEntityAttributesPaginatedAsync(
            string? clientCode = null,
            string? entityType = null,
            string? searchKey = null,
            string? entityKey = null,
            string? title = null,
            int page = 1,
            int pageSize = 10);
    }
} 