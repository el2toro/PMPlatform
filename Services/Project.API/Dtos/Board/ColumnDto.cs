namespace Project.API.Dtos.Board;

public class ColumnDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public Guid BoardId { get; set; }
    public IEnumerable<TaskItemDto> Tasks { get; set; } = new List<TaskItemDto>();
}
