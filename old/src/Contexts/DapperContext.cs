using Libs;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Contexts
{
    public interface IDapperContext
    {
        IDbConnection CreateConnection();
    }

    public class DapperContext : IDapperContext
    {
        private readonly IConfigurationUtils _configurationUtils;
        private readonly string _connectionString = "";
        public DapperContext(IConfigurationUtils configurationUtils)
        {
            _configurationUtils = configurationUtils;
            _connectionString = configurationUtils.GetConfiguration("AppSettings:ConnectionString");
        }

        public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}
