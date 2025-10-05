namespace Project.API.Models;

public class BoardColumn
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public Guid BoardId { get; set; }
    public Board Board { get; set; } = default!;
    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}
