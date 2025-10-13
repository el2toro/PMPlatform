namespace Project.API.Repository;

public interface IProjectRepository
{
    Task<Models.Project> CreateProjectAsync(Guid projectId, Models.Project project, CancellationToken cancellationToken);
    Task<(IEnumerable<Models.Project>, int)> GetProjectsAsync(Guid tenantdId, int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<Models.Project> GetProjectByIdAsync(Guid tenantId, Guid projectId, CancellationToken cancellationToken);
    Task<Models.Project> UpdateProjectAsync(ProjectDto projectDto, CancellationToken cancellationToken);
    Task DeleteProjectAsync(Guid tenantdId, Guid projectId, CancellationToken cancellationToken);
}
