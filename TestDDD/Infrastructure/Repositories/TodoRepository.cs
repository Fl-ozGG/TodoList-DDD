using Microsoft.EntityFrameworkCore;
using TestDDD.Domain.Entities;
using TestDDD.Domain.Enums;
using TestDDD.Domain.Repositories;
using TestDDD.Infrastructure.Persistance;

namespace TestDDD.Infrastructure.Repositories;

public class TodoRepository : ITodoRepository
{
    private readonly TodoDbContext _context;

    public TodoRepository(TodoDbContext context)
    {
        _context = context;
    }
    public async void AddTask(TodoItem task)
    {
        _context.TodoItems.Add(task);
        await _context.SaveChangesAsync();
    }

    public async Task<TodoItem?> GetTaskById(int id)
    {
        return await _context.TodoItems.FindAsync(id);
    }

    public async Task<IEnumerable<TodoItem>> GetAllTasks()
    {
        return await _context.TodoItems.ToListAsync();
    }

    public async Task<IEnumerable<TodoItem>> GetTasksByStatus(StatusEnum status)
    {
        var tasksList = await _context.TodoItems.ToListAsync();
        var taskByStatus = tasksList.Where(t => t.Status == status);
        return taskByStatus.ToList();
    }
    public async Task<TodoItem> UpdateTask(TodoItem task)
    {
        _context.Entry(task).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return task;
    }

    public async Task<bool> DeleteTask(int id)
    {
        var task = await GetTaskById(id);
        if (task == null)
        {
            throw new KeyNotFoundException($"Task with id {id} not found.");
        }
        _context.TodoItems.Remove(task);
        await _context.SaveChangesAsync();
        return true;
    }
     
}