using TestDDD.API.DTOs;
using TestDDD.Domain.Entities;
using TestDDD.Domain.Enums;
using TestDDD.Domain.Repositories;

namespace TestDDD.Domain.Services;

public class TodoServices(IUserRepository userRepository, ITodoRepository todoRepository)
{
    public async Task<TodoItem?> CreateTask(CreateTaskRequest request, int  userId)
    {
        var user = await userRepository.GetUserById(userId);
        if (user == null)
        {
            return null;
        }

        if (!user.HasLessThanFiveTasksAlive())
        {
            return null;
        }
        var task = new TodoItem
        {
            UserId = userId,
            Title = request.Title,
            Description = request.Description,
            Status = StatusEnum.Pending,
            CreatedAt = DateTime.Now
        };
        if (!user.HasLessThanThreeTasksRunning())
        {
            return null;
        }
        todoRepository.AddTask(task);
        return task;
    }

    public async Task<TodoItem?> UpdateTask(int taskId, UpdateTaskRequest newTask)
    {
        var oldTask = await todoRepository.GetTaskById(taskId);
        if (oldTask == null)
        {
            return null;
        }

        oldTask.Title = newTask.Title;
        oldTask.Description = newTask.Description;
        oldTask.Status = newTask.Status;

        var updatedTask = await todoRepository.UpdateTask(oldTask);
        return updatedTask;
    }

    public async Task<bool> DeleteTask(int taskId)
    {
        var result = await todoRepository.DeleteTask(taskId);
        if (!result)
        {
            return false;
        }
        return true;
        
    }

    public async Task<TodoItem?> GetTaskById(int id)
    {
        return await todoRepository.GetTaskById(id);
    }

    public async Task<IEnumerable<TodoItem>> GetAllTasks()
    {
        return await todoRepository.GetAllTasks();
    }

    public async Task<IEnumerable<TodoItem>> GetTasksByStatus(StatusEnum status)
    {
        return await todoRepository.GetTasksByStatus(status);
    }
}

























