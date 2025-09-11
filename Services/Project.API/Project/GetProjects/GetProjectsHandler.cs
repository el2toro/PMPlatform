using MediatR;
using Project.API.Dtos;
using Project.API.Repository;

namespace Project.API.Project.GetProjects;

public record GetProjectsQuery : IRequest<ProjectResponse>;
public record ProjectResponse(IEnumerable<ProjectDto> Projects);
public class GetProjectsHandler(IProjectRepository projectRepository)
    : IRequestHandler<GetProjectsQuery, ProjectResponse>
{
    public async Task<ProjectResponse> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
    {
        var projects = await projectRepository.GetProjectsAsync();
        return new ProjectResponse(projects);
    }
}
