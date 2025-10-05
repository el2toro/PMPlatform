namespace Board.Application.Dtos;

public record ColumnDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public Guid BoardId { get; set; }
}
