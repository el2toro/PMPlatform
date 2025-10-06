namespace Project.API.Repository;

public interface IProjectRepository
{
    Task<Models.Project> CreateProjectAsync(
        string name,
        string? description,
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken);
    Task<(IEnumerable<Models.Project>, int)> GetProjectsAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<ProjectDetailsDto> GetProjectDetailsAsync(Guid projectId, Guid tenantId, CancellationToken cancellationToken);
    Task<Models.Project> UpdateProjectAsync(ProjectDto projectDto, CancellationToken cancellationToken);
}
