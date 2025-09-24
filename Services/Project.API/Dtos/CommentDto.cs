namespace Project.API.Dtos;

public record CommentDto
{
    public Guid TaskId { get; set; }

    public string Content { get; set; } = default!;
}