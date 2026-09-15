using Dapper;
using TodoApp.Web.Data;
using TodoApp.Web.Models;

namespace TodoApp.Web.Repositories; 

public class TaskRepository
{
    private readonly DapperContext _context; 

    public TaskRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TaskItem>> GetAllAsync()
    {
        const string sql = "SELECT Id, Title, Notes, Status, DueDate FROM Tasks"; 
        using var connection = _context.CreateConnection(); 
        return await connection.QueryAsync<TaskItem>(sql); 
    }

    public async Task<TaskItem?> GetByIdAsync(int id)
    {
        const string sql = "SELECT Id, Title, Notes, Status, DueDate FROM Tasks WHERE Id = @Id"; 
        using var connection = _context.CreateConnection(); 
        return await connection.QuerySingleOrDefaultAsync<TaskItem>(sql, new {Id = id}); 
    }

    public async Task<int> CreateAsync(TaskItem task)
    {
        const string sql = """
            INSERT INTO Tasks (Title, Notes, Status, DueDate)
            VALUES (@Title, @Notes, @Status, @DueDate);
            SELECT LAST_INSERT_ID();
            """; 
        using var connection = _context.CreateConnection(); 
        return await connection.ExecuteScalarAsync<int>(sql, new
        {
            task.Title, 
            task.Notes, 
            Status = task.Status.ToString(), 
            task.DueDate
        }); 
    }

    public async Task<bool> UpdateAsync(TaskItem task)
    {
        const string sql = """
            UPDATE Tasks 
            SET Title = @Title, Notes = @Notes, Status = @Status, DueDate = @DueDate
            WHERE Id = @Id
            """; 
        using var connection = _context.CreateConnection(); 
        var rowsAffected = await connection.ExecuteAsync(sql, new
        {
            task.Id,
            task.Title,
            task.Notes, 
            Status = task.Status.ToString(), 
            task.DueDate
        }); 
        return rowsAffected > 0; 
    }

    public async Task<bool> DeleteAsync(int id)
    {
        const string sql = "DELETE FROM Tasks WHERE Id = @Id"; 
        using var connection = _context.CreateConnection(); 
        var rowsAffected = await connection.ExecuteAsync(sql, new { Id = id }); 
        return rowsAffected > 0; 
    }
}