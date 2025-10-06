namespace TaskService.Domain.Entities;

public class Comment
{
    public Guid Id { get; set; }
    public Guid CommentedBy { get; set; }

    public string Content { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation
    public Guid TaskId { get; set; }
    public TaskItem Task { get; set; } = default!;
}
