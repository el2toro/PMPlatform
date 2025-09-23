using MediatR;
using Project.API.Dtos;
using Project.API.Repository;
using Project.API.Services;

namespace Project.API.Project.GetProjectById;

public record GetProjectByIdQuery(Guid ProjectId, Guid TenantId) : IRequest<GetProjectByIdResult>;
public record GetProjectByIdResult(ProjectDetailsDto Project);

public class GetProjectByIdHandler(IProjectRepository projectRepository, UserServiceClient userServiceClient)
    : IRequestHandler<GetProjectByIdQuery, GetProjectByIdResult>
{
    public async Task<GetProjectByIdResult> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        var project = await projectRepository.GetProjectDetailsAsync(request.ProjectId, request.TenantId, cancellationToken);

        var createdByIds = project.Tasks.Select(t => t.CreatedBy).Distinct().ToArray();

        var createdByUsers = await userServiceClient.GetUsersByIdAsync(request.TenantId, createdByIds);

        foreach (var task in project.Tasks)
        {
            task.User = createdByUsers.SingleOrDefault(u => u.Id == task.CreatedBy) ?? new UserDto();
        }

        return new GetProjectByIdResult(project);
    }
}
