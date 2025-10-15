namespace TaskService.Application.Dtos;

public record CommentDto
{
    public Guid Id { get; set; }
    public Guid TaskId { get; set; }
    public Guid CommentedBy { get; set; }

    public string Content { get; set; } = default!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
