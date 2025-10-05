namespace Board.Application.Dtos;

public class BoardDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; } = default!;
    public Guid CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid ProjectId { get; set; }
    public string ProjectName { get; set; } = default!;
    public string? ProjectDescription { get; set; } = default!;
    public IEnumerable<ColumnDto> Columns { get; set; } = [];
}
