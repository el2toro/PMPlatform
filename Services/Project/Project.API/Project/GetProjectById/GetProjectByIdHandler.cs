using Core.Services;

namespace Project.API.Project.GetProjectById;

public record GetProjectByIdQuery(Guid ProjectId, Guid TenantId) : IRequest<GetProjectByIdResult>;
public record GetProjectByIdResult(ProjectDto Project);

public class GetProjectByIdHandler(IProjectRepository projectRepository,
    ICacheService cacheService)
    : IRequestHandler<GetProjectByIdQuery, GetProjectByIdResult>
{
    public async Task<GetProjectByIdResult> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
    {
        var cachedProject = await cacheService.GetAsync<ProjectDto>(request.ProjectId.ToString(), cancellationToken);

        if (cachedProject is not null)
        {
            return new GetProjectByIdResult(cachedProject);
        }

        var project = await projectRepository.GetProjectByIdAsync(request.TenantId, request.ProjectId, cancellationToken);
        var projectDto = project.Adapt<ProjectDto>();

        await cacheService.SetAsync<ProjectDto>(request.ProjectId.ToString(),
            projectDto,
            cancellationToken,
            TimeSpan.FromHours(1));

        return new GetProjectByIdResult(projectDto);
    }
}
