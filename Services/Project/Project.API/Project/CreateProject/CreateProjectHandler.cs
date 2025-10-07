using Project.API.TenantContext;

namespace Project.API.Project.CreateProject;

public record CreateProjectCommand(Guid tenantdId,
    string Name,
    string? Description,
    DateTime StartDate,
    DateTime EndDate) : IRequest<CreateProjectResult>;
public record CreateProjectResult(ProjectDto ProjectDto);

public class CreateProjectHandler(IProjectRepository projectRepository,
    ITenantContext tenantContext,
    IPublishEndpoint publishEndpoint)
    : IRequestHandler<CreateProjectCommand, CreateProjectResult>
{
    public async Task<CreateProjectResult> Handle(CreateProjectCommand command, CancellationToken cancellationToken)
    {
        var project = MapCommandToProject(command, tenantContext);
        var createdProject = await projectRepository.CreateProjectAsync(project, cancellationToken);
        var result = project.Adapt<ProjectDto>();

        await publishEndpoint.Publish<ProjectCreatedEvent>(result);

        return new CreateProjectResult(result);
    }

    private Models.Project MapCommandToProject(CreateProjectCommand command, ITenantContext tenantContext)
    {
        var project = new Models.Project
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

        return project;
    }
}
