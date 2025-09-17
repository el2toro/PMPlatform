using Project.API.Enums;

namespace Project.API.Dtos;

public record ProjectDetailsDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public Guid TenantId { get; set; }
    public ProjectStatus ProjectStatus { get; set; }
    public DateTime EndDate { get; set; }
    public IEnumerable<TaskItemDto> Tasks { get; set; } = new List<TaskItemDto>();
}

