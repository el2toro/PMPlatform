using MediatR;
using Project.API.Repository;

namespace Project.API.Project.UpdateProject;

public record UpdateProjectCommand(Guid ProjectId, string Name, string Description) : IRequest<UpdateProjectResult>;
public record UpdateProjectResult(bool IsSuccefull);
public class UpdateProjectHandler(IProjectRepository projectRepository)
    : IRequestHandler<UpdateProjectCommand, UpdateProjectResult>
{
    public async Task<UpdateProjectResult> Handle(UpdateProjectCommand command, CancellationToken cancellationToken)
    {
        //var result = await projectRepository.UpdateProjectAsync(command.ProjectId, command.Name, command.Description, cancellationToken);

        throw new NotImplementedException();
    }
}
