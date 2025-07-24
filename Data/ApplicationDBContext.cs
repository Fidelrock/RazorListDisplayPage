using Microsoft.EntityFrameworkCore;
using RazorTableDemo.Models;

namespace RazorTableDemo.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options): base(options)
        {
        }

        

    }
}
