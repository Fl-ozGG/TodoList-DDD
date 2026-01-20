using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TestDDD.API.DTOs;
using TestDDD.Domain.Entities;
using TestDDD.Domain.Repositories;
using TestDDD.Infrastructure.Persistance;

namespace TestDDD.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    
    private readonly TodoDbContext _context;

    public UserRepository(TodoDbContext context)
    {
        _context = context;
    }
    //Todo ignacio añadir la implementacion de la interfaz
    
    public async Task<User?> GetUserById(int userId)
    {
        var user = await _context.Users
            .Include(u => u.Tasks)
            .FirstOrDefaultAsync(u => u.Id == userId);
        return user;
    }

    public async Task AssignTaskToUser(User user, TodoItem task)
    {
        Console.WriteLine(user);
        user.Tasks.Add(task);
        await _context.SaveChangesAsync();
    }

    public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User?> AddUser(CreateUserRequest request)
    {
        var newUser = new User(request.Username);
        _context.Users.Add(newUser);
        await _context.SaveChangesAsync();
        return newUser;
    }
    
    
}