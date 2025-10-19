namespace TaskService.Domain.Entities;

public class TaskItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public DateTime DueDate { get; set; }
    public Guid ProjectId { get; set; }
    public Guid? AssignedTo { get; set; }
    public TaskItemStatus TaskStatus { get; set; } = TaskItemStatus.Backlog;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public Guid UpdatedBy { get; set; }

    // Navigation
    public List<Subtask>? Subtasks { get; set; } = default!;
    public List<Comment>? Comments { get; set; } = default!;
    public List<Attachment>? Attachments { get; set; } = default!;
}
