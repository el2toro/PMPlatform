namespace TaskService.Application.Dtos;

public record SubtaskDto
{
    public Guid Id { get; set; }
    public Guid TaskId { get; set; }
    public string Title { get; set; } = default!;
    public bool IsCompleted { get; set; }
}
