using Project.API.Enums;

namespace Project.API.Models;

public class TaskItem
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public DateTime DueDate { get; set; }
    public Guid ProjectId { get; set; }
    public Guid AssignedTo { get; set; }
    public TaskItemStatus TaskStatus { get; set; } = TaskItemStatus.Backlog;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public Guid UpdatedBy { get; set; }

    // Navigation
    public Guid ColumnId { get; set; }
    public BoardColumn Column { get; set; } = default!;
    public Project Project { get; set; } = default!;
    public ICollection<Subtask> Subtasks { get; set; } = new List<Subtask>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
}
