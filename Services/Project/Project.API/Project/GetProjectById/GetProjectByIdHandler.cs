namespace Project.API.Project.GetProjectById;

public record GetProjectByIdQuery(Guid ProjectId, Guid TenantId) : IQuery<GetProjectByIdResult>;
public record GetProjectByIdResult(ProjectDto Project);

public class GetProjectByIdHandler(IProjectRepository projectRepository,
    ICacheService cacheService)
    : IQueryHandler<GetProjectByIdQuery, GetProjectByIdResult>
{
    public async Task<GetProjectByIdResult> Handle(GetProjectByIdQuery query, CancellationToken cancellationToken)
    {
        var cachedProject = await cacheService.GetAsync<ProjectDto>(query.ProjectId.ToString(), cancellationToken);

        if (cachedProject is not null)
        {
            return new GetProjectByIdResult(cachedProject);
        }

        var project = await projectRepository.GetProjectByIdAsync(query.TenantId, query.ProjectId, cancellationToken);
        var projectDto = project.Adapt<ProjectDto>();

        await cacheService.SetAsync<ProjectDto>(query.ProjectId.ToString(),
            projectDto,
            cancellationToken,
            TimeSpan.FromHours(1));

        return new GetProjectByIdResult(projectDto);
    }
}
