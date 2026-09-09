using System.Data; 
using MySqlConnector; 

namespace TodoApp.Web.Data; 

public class DapperContxt
{
    private readonly string _connectionString; 
    public DapperContxt(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("Default Connection")
            ?? throw new InvalidOperationException("Connection string 'Default Connection' not found.")
    }
    public IDbConnection CreateConnection()
        => new MySqlConnection(_connectionString);
}