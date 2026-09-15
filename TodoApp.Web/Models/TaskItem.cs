namespace TodoApp.Web.Models; 

public enum TodoStatus
{
    Open,
    InProgress, 
    Done
}

public class TaskItem
{
    public int Id { get; set; }
    public string Title { get; set; } = "New Task"; 
    public string? Notes { get; set; }
    public TodoStatus Status { get; set; } = TodoStatus.Open; 
    public DateTime? DueDate { get; set;  }
}