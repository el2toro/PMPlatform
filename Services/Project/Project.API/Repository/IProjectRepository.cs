namespace Project.API.Repository;

public interface IProjectRepository
{
    Task<Models.Project> CreateProjectAsync(Models.Project project, CancellationToken cancellationToken);
    Task<(IEnumerable<Models.Project>, int)> GetProjectsAsync(Guid tenantdId, int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<ProjectDetailsDto> GetProjectDetailsAsync(Guid projectId, Guid tenantId, CancellationToken cancellationToken);
    Task<Models.Project> UpdateProjectAsync(ProjectDto projectDto, CancellationToken cancellationToken);
}
