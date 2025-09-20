using Mapster;
using MediatR;
using Project.API.Dtos;
using Project.API.Repository;

namespace Project.API.Project.UpdateProject;

public record UpdateProjectCommand(ProjectDto ProjectDto) : IRequest<UpdateProjectResult>;
public record UpdateProjectResult(ProjectDto ProjectDto);
public class UpdateProjectHandler(IProjectRepository projectRepository)
    : IRequestHandler<UpdateProjectCommand, UpdateProjectResult>
{
    public async Task<UpdateProjectResult> Handle(UpdateProjectCommand command, CancellationToken cancellationToken)
    {
        // var project = command.ProjectDto.Adapt<Models.Project>();
        var result = await projectRepository.UpdateProjectAsync(command.ProjectDto, cancellationToken);

        return new UpdateProjectResult(MapToDto(command.ProjectDto.Progress, command.ProjectDto.Team, result));
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
