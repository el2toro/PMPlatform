namespace Project.API.Dtos;

public record ProjectProgressDto
{
    public int TotalTasks { get; set; }
    public int CompletedTasks { get; set; }
    public int TotalSubtasks { get; set; }
    public int CompletedSubtasks { get; set; }
}
