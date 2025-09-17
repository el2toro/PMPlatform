using Project.API.Dtos;

namespace Project.API.Repository;

public interface IProjectRepository
{
    Task<Models.Project> CreateProjectAsync(string name, string description, CancellationToken cancellationToken);
    Task<IEnumerable<ProjectDto>> GetProjectsAsync(CancellationToken cancellationToken);
    Task<ProjectDetailsDto> GetProjectDetailsAsync(Guid projectId, Guid tenantId, CancellationToken cancellationToken);
}
