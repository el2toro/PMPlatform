namespace Project.API.Project.CreateProject;

public record CreateProjectCommand(Guid TenantdId,
    string Name,
    string? Description,
    DateTime StartDate,
    DateTime EndDate) : ICommand<CreateProjectResult>;
public record CreateProjectResult(ProjectDto ProjectDto);

public class CreateProjectHandler(IProjectRepository projectRepository,
    TenantAwareContextService tenantContext,
    IPublishEndpoint publishEndpoint)
    : ICommandHandler<CreateProjectCommand, CreateProjectResult>
{
    public async Task<CreateProjectResult> Handle(CreateProjectCommand command, CancellationToken cancellationToken)
    {
        var projectToBeCreated = MapCommandToProject(command, tenantContext);
        var createdProject = await projectRepository.CreateProjectAsync(new Models.Project(), cancellationToken);

        var projectDto = createdProject.Adapt<ProjectDto>();

        await publishEndpoint.Publish<ProjectCreatedEvent>(projectDto, cancellationToken);

        return new CreateProjectResult(projectDto);
    }

    private Models.Project MapCommandToProject(CreateProjectCommand command, ITenantContext tenantContext) => new()
    {
        Name = command.Name,
        Description = command.Description,
        StartDate = command.StartDate,
        EndDate = command.EndDate,
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow,
        CreatedBy = tenantContext.UserId,
        UpdatedBy = tenantContext.UserId,
        TenantId = tenantContext.TenantId,
        ProjectStatus = ProjectStatus.NotStarted
    };
}
