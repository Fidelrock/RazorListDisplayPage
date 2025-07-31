using RazorTableDemo.Models;

namespace RazorTableDemo.Services
{
    public interface IETIMSequenceService
    {
        Task<(IEnumerable<ETIMSequence> Results, int TotalCount, int TotalPages)> GetETIMSequencesPaginatedAsync(
            string? clientCode = null,
            int page = 1,
            int pageSize = 10);
    }
} 