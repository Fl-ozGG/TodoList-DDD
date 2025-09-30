using TestDDD.Domain.Enums;

namespace TestDDD.Domain.Entities;

public class User
{
    public User(string username)
    {
        Username = username;
    }

    public int Id { get; set; }
    public string Username { get; set; }
    
    public List<TodoItem> Tasks { get; set; } = new();
    
    
    
    public bool HasLessThanThreeTasksRunning()
    {
        var taskRuning = Tasks.Count(task => task.Status == StatusEnum.InProgress);
        return taskRuning < 3;
    }
    public bool HasLessThanFiveTasksAlive()
    {
        return 5 > Tasks.Count(t => t.Status is StatusEnum.Pending or StatusEnum.InProgress);

    }
    
}