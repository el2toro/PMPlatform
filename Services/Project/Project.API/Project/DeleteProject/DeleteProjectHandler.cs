namespace Project.API.Project.DeleteProject;

public record DeleteProjectCommand(Guid TenantId, Guid ProjectId) : ICommand<DeleteProjectResult>;
public record DeleteProjectResult(bool IsSuccess);

public class DeleteProjectHandler(IProjectRepository projectRepository)
    : ICommandHandler<DeleteProjectCommand, DeleteProjectResult>
{
    public async Task<DeleteProjectResult> Handle(DeleteProjectCommand command, CancellationToken cancellationToken)
    {
        await projectRepository.DeleteProjectAsync(command.TenantId, command.ProjectId, cancellationToken);
        return new DeleteProjectResult(true);
    }
}
