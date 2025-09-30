using TestDDD.Domain.Enums;

namespace TestDDD.API.DTOs;

public class CreateTaskRequest
{
    public required string Title { get; set; }
    public required string Description { get; set; }
   
    public int UserId { get; set; }
}