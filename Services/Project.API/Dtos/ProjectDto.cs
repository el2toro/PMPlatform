using Project.API.Enums;

namespace Project.API.Dtos;

public record ProjectDto(
    Guid Id,
    string Name,
    string? Description,
    DateTime CreatedAt,
    Guid CreatedBy,
    Guid TenantId,
    ProjectStatus ProjectStatus,
    DateTime EndDate,
    int Progress,
    IEnumerable<UserDto> Team);

