namespace Project.API.Dtos;

public record CommentDto
{
    public Guid Id { get; set; }
    public Guid TaskId { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }

    public string Content { get; set; } = default!;
}