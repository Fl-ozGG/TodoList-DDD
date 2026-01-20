using Microsoft.AspNetCore.Mvc;
using TestDDD.API.DTOs;
using TestDDD.Domain.Entities;
using TestDDD.Domain.Repositories;

namespace TestDDD.Domain.Services;

public class UserServices(IUserRepository userRepository, ITodoRepository todoRepository)
{
    public async Task<User?> CreateUser(CreateUserRequest request)
    {
        return await userRepository.AddUser(request);
    }

    public async Task<User?> GetUserById(int userId)
    {
        return await userRepository.GetUserById(userId);
    }
    
    public async Task<bool> AssignTask(int userId, int taskId)
    {
        var user = await userRepository.GetUserById(userId);
        var task = await todoRepository.GetTaskById(taskId);
        await userRepository.AssignTaskToUser(user, task);
        return true;
    }

    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        return await userRepository.GetAllUsers();
    }
}