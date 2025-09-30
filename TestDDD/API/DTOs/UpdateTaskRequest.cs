using TestDDD.Domain.Enums;

namespace TestDDD.API.DTOs;

public class UpdateTaskRequest
{
    public StatusEnum Status { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    
}