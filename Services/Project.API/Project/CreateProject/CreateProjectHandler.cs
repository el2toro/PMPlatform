using Mapster;
using MediatR;
using Project.API.Dtos;
using Project.API.Repository;

namespace Project.API.Project.CreateProject;

public record CreateProjectCommand(string Name, string Description, string StartDate) : IRequest<CreateProjectResult>;
public record CreateProjectResult(ProjectDto ProjectDto);

public class CreateProjectHandler(IProjectRepository projectRepository)
    : IRequestHandler<CreateProjectCommand, CreateProjectResult>
{
    public async Task<CreateProjectResult> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await projectRepository.CreateProjectAsync(request.Name, request.Description, cancellationToken);
        return new CreateProjectResult(project.Adapt<ProjectDto>());
    }
}
