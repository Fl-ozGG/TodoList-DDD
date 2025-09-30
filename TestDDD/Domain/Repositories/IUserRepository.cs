using Microsoft.AspNetCore.Mvc;
using TestDDD.API.DTOs;
using TestDDD.Domain.Entities;

namespace TestDDD.Domain.Repositories;

public interface IUserRepository
{
    Task<User?> AddUser(CreateUserRequest request);
    Task<User?> GetUserById(int userId);
    void AssignTaskToUser(User user, TodoItem task);
    Task<ActionResult<IEnumerable<User>>> GetAllUsers();

}