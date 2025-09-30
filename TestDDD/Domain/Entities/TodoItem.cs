using System.Text.Json.Serialization;
using TestDDD.Domain.Enums;


namespace TestDDD.Domain.Entities;

public class TodoItem
{
    public int Id { get; set; }
    public string? Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public StatusEnum Status { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public int UserId { get; set; }
    [JsonIgnore]
    public User? User { get; set; }
}