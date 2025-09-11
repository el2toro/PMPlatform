using MediatR;
using Project.API.Repository;

namespace Project.API.Project.CreateProject;

public record CreateProjectCommand(string Name, string Description) : IRequest<CreateProjectResult>;
public record CreateProjectResult(int ProjectId);

public class CreateProjectHandler(IProjectRepository projectRepository)
    : IRequestHandler<CreateProjectCommand, CreateProjectResult>
{
    public async Task<CreateProjectResult> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        await projectRepository.CreateProject(request.Name, request.Description, cancellationToken);
        return new CreateProjectResult(1);
    }
}
