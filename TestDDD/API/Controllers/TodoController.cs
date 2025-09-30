using Microsoft.AspNetCore.Mvc;
using TestDDD.Domain.Entities;
using TestDDD.Domain.Enums;
using TestDDD.Domain.Services;
using TestDDD.Infrastructure.Repositories;

namespace TestDDD.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class TodoController(TodoServices todoServices) : ControllerBase
{
    [HttpPost("create-task")]
    public async Task<ActionResult<TodoItem>> CreateTask(DTOs.CreateTaskRequest request, int userId)
    {
        var task = await todoServices.CreateTask(request,  userId);
        if (task == null)
        {
            return BadRequest("No se pudo crear la tarea por reglas de negocio.");
        }
        return CreatedAtAction(nameof(GetTask), new { id = task.Id, UserId = userId }, task);
    }
    
    [HttpPut("update-task/{taskId}")]
    public async Task<ActionResult<TodoItem>> UpdateTask(int taskId, DTOs.UpdateTaskRequest newTask)
    {
        var taskUpdated = await todoServices.UpdateTask(taskId, newTask);
        if (taskUpdated == null)
        {
            return BadRequest();
        }
        return Ok(taskUpdated);
    }

    [HttpDelete("delete-task/{taskId}")]
    public async Task<IActionResult> DeleteTask(int taskId)
    {   
        var success = await todoServices.DeleteTask(taskId);
        if (!success)
        {
            return NotFound();
        }
        return NoContent();
    }

    [HttpGet("task/{id}")]
    public async Task<ActionResult<TodoItem>> GetTask(int id)
    {
        var task = await todoServices.GetTaskById(id);
        return task == null ? NotFound() : Ok(task);    
    }

    [HttpGet("all-tasks")]
    public async Task<ActionResult<IEnumerable<TodoItem>>> GetTasks()
    {
        // todo ignacio deberia llamar al service
        var tasks =  await todoServices.GetAllTasks();
        return Ok(tasks);
    }    
    
    [HttpGet("tasks-by-status/{status}")]
    public async Task<ActionResult<IEnumerable<TodoItem>>> GetTasksByStatus(StatusEnum status)
    {
        var taskListByStatus = await todoServices.GetTasksByStatus(status);
        return Ok(taskListByStatus);
    }
    

}






















