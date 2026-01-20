using Microsoft.AspNetCore.Mvc;
using TestDDD.Domain.Entities;
using TestDDD.Domain.Services;


namespace TestDDD.API.Controllers;

[ApiController]
[Route("api/[controller]")]

public class UsersController : ControllerBase
{
    private readonly UserServices _userServices;

    public UsersController(UserServices userServices)
    {
        _userServices = userServices;
    }
    [HttpPost("create-user")]
    public async Task<ActionResult<string>> CreateUser(DTOs.CreateUserRequest request)
    {
        var userCreated = await _userServices.CreateUser(request);
        if (userCreated == null)
        {
            return NotFound();
        }
        return CreatedAtAction(nameof(GetUserById), new { userId = userCreated.Id }, userCreated);
    }

    [HttpGet("get-user/{userId}")]
    public async Task<ActionResult<User>> GetUserById(int userId)
    {
        var userRequested = await _userServices.GetUserById(userId);
        if (userRequested == null)
        {
            return NotFound();
        }
        return userRequested;
    }
    
    [HttpPut("assign-task/{userId}/{taskId}")]
    public async Task<bool> AssignTaskToUser(int userId, int taskId)
    {
        var response = await _userServices.AssignTask(userId, taskId);
        return response;
    }
    [HttpGet("get-all-users")]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        var allUsers = await _userServices.GetUsers();
        return Ok(allUsers);
    }
}