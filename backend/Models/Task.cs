using System.ComponentModel.DataAnnotations;

namespace TodoApp.Models;

public class TaskModel
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string Title { get; set; } = string.Empty;

    public string? Description { get; set; }

    public string[] Tags { get; set; } = Array.Empty<string>();

    public DateTime CreatedAt { get; set; }

    public DateTime ModifiedAt { get; set; }
}