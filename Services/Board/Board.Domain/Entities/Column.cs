namespace Board.Domain.Entities;

public class Column
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public Guid BoardId { get; set; }
    public Board Board { get; set; } = default!;
}
