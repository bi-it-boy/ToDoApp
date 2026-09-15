using Microsoft.Extensions.Configuration;
using TodoApp.Web.Data;
using TodoApp.Web.Repositories;
using TodoApp.Web.Models; 
using Xunit;

namespace ToDoApp.Tests;

public class TaskRepositoryTests
{
    private static TaskRepository CreateRepository()
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:DefaultConnection"] =
                    "Server=localhost;Port=3307;Database=todoapp;User=todoapp_user;Password=todoapp_pass;"
            })
            .Build();

        var context = new DapperContext(config);
        return new TaskRepository(context);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsTasksWithoutThrowing()
    {
        var repository = CreateRepository();

        var tasks = await repository.GetAllAsync();

        Assert.NotNull(tasks);
    }

    [Fact]
    public async Task CreateAsync_ThenGetById_ReturnsCreatedTask()
    {
        var repository = CreateRepository();
        var newTask = new TaskItem { Title = "Test Task", Status = TodoStatus.Open };

        var newId = await repository.CreateAsync(newTask);
        var fetched = await repository.GetByIdAsync(newId);

        Assert.NotNull(fetched);
        Assert.Equal("Test Task", fetched!.Title);

        await repository.DeleteAsync(newId); // Aufräumen
    }

    [Fact]
    public async Task UpdateAsync_ChangesTitle()
    {
        var repository = CreateRepository();
        var newId = await repository.CreateAsync(new TaskItem { Title = "Original" });

        var updated = new TaskItem { Id = newId, Title = "Updated", Status = TodoStatus.InProgress };
        var success = await repository.UpdateAsync(updated);
        var fetched = await repository.GetByIdAsync(newId);

        Assert.True(success);
        Assert.Equal("Updated", fetched!.Title);
        Assert.Equal(TodoStatus.InProgress, fetched.Status);

        await repository.DeleteAsync(newId); // Aufräumen
    }

    [Fact]
    public async Task DeleteAsync_RemovesTask()
    {
        var repository = CreateRepository();
        var newId = await repository.CreateAsync(new TaskItem { Title = "To Delete" });

        var success = await repository.DeleteAsync(newId);
        var fetched = await repository.GetByIdAsync(newId);

        Assert.True(success);
        Assert.Null(fetched);
    }

    [Fact]
    public async Task DeleteAsync_NonExistentId_ReturnsFalse()
    {
        var repository = CreateRepository();

        var success = await repository.DeleteAsync(999999);

        Assert.False(success);
    }
}