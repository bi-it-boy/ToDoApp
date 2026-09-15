using System.Data; 
using MySqlConnector; 

namespace TodoApp.Web.Data; 

public class DapperContext
{
    private readonly string _connectionString; 
    public DapperContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'Default Connection' not found.");
    }
    public IDbConnection CreateConnection()
        => new MySqlConnection(_connectionString);
}