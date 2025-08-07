using Dapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RazorTableDemo.Models;
using System.Data;

namespace RazorTableDemo.Services
{
    public abstract class BaseDataService : IDisposable
    {
        protected readonly IConfiguration _configuration;
        protected readonly ILogger _logger;
        protected readonly AppSettings _appSettings;
        protected readonly IDbConnection _connection;
        protected bool _disposed = false;

        protected BaseDataService(
            IConfiguration configuration, 
            ILogger logger, 
            IOptions<AppSettings> appSettings)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _appSettings = appSettings?.Value ?? throw new ArgumentNullException(nameof(appSettings));
            
            var connectionString = _configuration.GetConnectionString("DefaultConnection") 
                ?? throw new InvalidOperationException("DefaultConnection string not found in configuration");
            
            _connection = new Microsoft.Data.SqlClient.SqlConnection(connectionString);
            
            _logger.LogInformation("{ServiceName} initialized for client: {ClientCode}", 
                GetType().Name, _appSettings.ClientCode);
        }

        protected void ValidatePaginationParameters(ref int page, ref int pageSize)
        {
            if (page < 1) page = 1;
            if (pageSize < 1 || pageSize > _appSettings.MaxPageSize) 
                pageSize = _appSettings.DefaultPageSize;
        }

        protected string GetClientCodeWhereClause()
        {
            return "WHERE ClientCode = @ClientCode";
        }

        protected void AddClientCodeParameter(DynamicParameters parameters)
        {
            parameters.Add("ClientCode", _appSettings.ClientCode);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _connection?.Dispose();
                _disposed = true;
            }
        }
    }
} 