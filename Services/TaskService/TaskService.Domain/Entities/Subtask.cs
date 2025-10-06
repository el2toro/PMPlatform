namespace TaskService.Domain.Entities;

public class Subtask
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public bool IsCompleted { get; set; }

    // Navigation
    public Guid TaskId { get; set; }
    public TaskItem Task { get; set; } = default!;
}
