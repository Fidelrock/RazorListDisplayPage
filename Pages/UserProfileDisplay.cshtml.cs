using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Dapper;
using Microsoft.Data.SqlClient;
using RazorTableDemo.Models;

public class UserProfileDisplayModel : PageModel
{
    private readonly IConfiguration _configuration;
    public UserProfileDisplayModel(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string SuccessMessage { get; set; }

    [BindProperty(SupportsGet = true)]
    public string ClientCode { get; set; }
    [BindProperty(SupportsGet = true)]
    public string AuthorityKey { get; set; }

    public List<S300TaxAuthority> Results { get; set; } = new List<S300TaxAuthority>();

    public void OnGet()
    {
        using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
        {
            var sql = "SELECT * FROM S300TaxAuthority WHERE 1=1";
            var parameters = new DynamicParameters();

            if (!string.IsNullOrEmpty(ClientCode))
            {
                sql += " AND ClientCode LIKE @ClientCode";
                parameters.Add("ClientCode", $"%{ClientCode}%");
            }
            if (!string.IsNullOrEmpty(AuthorityKey))
            {
                sql += " AND AuthorityKey LIKE @AuthorityKey";
                parameters.Add("AuthorityKey", $"%{AuthorityKey}%");
            }

            Results = connection.Query<S300TaxAuthority>(sql, parameters).ToList();
        }
    }
}