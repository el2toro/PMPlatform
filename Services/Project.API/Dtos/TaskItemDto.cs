using Project.API.Enums;

namespace Project.API.Dtos;

public record TaskItemDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    public DateTime DueDate { get; set; }
    public Guid ProjectId { get; set; }
    public Guid AssignedTo { get; set; }
    public TaskItemStatus TaskStatus { get; set; } = TaskItemStatus.Backlog;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public Guid UpdatedBy { get; set; }

    public UserDto User { get; set; } = default!;
    public IEnumerable<SubtaskDto> Subtasks { get; set; } = new List<SubtaskDto>();
}