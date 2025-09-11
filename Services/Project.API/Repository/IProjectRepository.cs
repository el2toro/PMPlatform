using Project.API.Dtos;

namespace Project.API.Repository;

public interface IProjectRepository
{
    Task CreateProject(string name, string description, CancellationToken cancellationToken);
    Task<IEnumerable<ProjectDto>> GetProjectsAsync();
}
