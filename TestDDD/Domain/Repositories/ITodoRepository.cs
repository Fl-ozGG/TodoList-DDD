using TestDDD.Domain.Entities;
using TestDDD.Domain.Enums;

namespace TestDDD.Domain.Repositories;

public interface ITodoRepository
{
    void AddTask(TodoItem task);
    Task<TodoItem?> GetTaskById(int id);
    Task<TodoItem> UpdateTask(TodoItem task);
    Task<bool> DeleteTask(int taskId);
    Task<IEnumerable<TodoItem>> GetAllTasks();
    Task<IEnumerable<TodoItem>> GetTasksByStatus(StatusEnum status);
}