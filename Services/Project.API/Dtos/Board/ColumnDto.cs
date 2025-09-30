using Project.API.Models;

namespace Project.API.Dtos.Board;

public class ColumnDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public Guid BoardId { get; set; }
    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}
