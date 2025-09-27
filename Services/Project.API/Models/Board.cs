namespace Project.API.Models;

public class Board
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public Guid CteatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid ProjectId { get; set; }
    public Project Project { get; set; } = default!;
    public ICollection<BoardColumn> Columns { get; set; } = new List<BoardColumn>();
}
