using Core.Services;

namespace Project.API.Project.UpdateProject;

public record UpdateProjectCommand(Guid TenantId, ProjectDto ProjectDto) : IRequest<UpdateProjectResult>;
public record UpdateProjectResult(ProjectDto ProjectDto);

//TODO: Add request validation behavior, check ir url tenantId == request tenantId
public class UpdateProjectHandler(IProjectRepository projectRepository,
    IPublishEndpoint publishEndpoint,
    ICacheService cacheService)
    : IRequestHandler<UpdateProjectCommand, UpdateProjectResult>
{
    public async Task<UpdateProjectResult> Handle(UpdateProjectCommand command, CancellationToken cancellationToken)
    {
        var project = await projectRepository.UpdateProjectAsync(command.ProjectDto, cancellationToken);
        var result = MapToDto(command.ProjectDto.Progress, command.ProjectDto.Team, project);

        await cacheService.DeleteAsync(command.ProjectDto.Id.ToString(), cancellationToken);
        await publishEndpoint.Publish<ProjectUpdatedEvent>(result, cancellationToken);

        return new UpdateProjectResult(result);
    }

    private ProjectDto MapToDto(int progress, IEnumerable<UserDto> team, Models.Project updatedProject)
    {
        return new ProjectDto
        {
            Id = updatedProject.Id,
            Name = updatedProject.Name,
            Description = updatedProject.Description,
            CreatedAt = updatedProject.CreatedAt,
            CreatedBy = updatedProject.CreatedBy,
            TenantId = updatedProject.TenantId,
            ProjectStatus = updatedProject.ProjectStatus,
            StartDate = updatedProject.StartDate,
            EndDate = updatedProject.EndDate,
            Progress = progress,
            Team = team
        };
    }
}
