namespace Project.API.Project.GetProjects;

public record GetProjectsQuery(Guid TenantId, int PageNumber, int PageSize) : IQuery<GetProjectsResult>;
public record GetProjectsResult(PaginatedResponse PaginatedResponse);

public class GetProjectsQueryValidator : AbstractValidator<GetProjectsQuery>
{
    public GetProjectsQueryValidator()
    {
        RuleFor(r => r.TenantId).NotEmpty().WithMessage("TenantId cannot be empty");
        RuleFor(r => r.TenantId).NotEqual(Guid.NewGuid()).WithMessage("Wrong TenantId");
    }
}

public class GetProjectsHandler(IProjectRepository projectRepository,
   ICacheService cacheService,
    UserServiceClient userServiceClient,
    TaskServiceClient taskServiceClient)
    : IQueryHandler<GetProjectsQuery, GetProjectsResult>
{
    public async Task<GetProjectsResult> Handle(GetProjectsQuery query, CancellationToken cancellationToken)
    {
        string PAGINATED_RESPONSE_KEY = $"paginated-response-{query.PageNumber}&{query.PageSize}";

        //var cachedPaginatedResponse = await cacheService.GetAsync<PaginatedResponse>(PAGINATED_RESPONSE_KEY, cancellationToken);
        //if (cachedPaginatedResponse is not null)
        //{
        //    return new GetProjectsResult(cachedPaginatedResponse);
        //}

        var (items, totalCount) = await projectRepository
            .GetProjectsAsync(query.TenantId, query.PageNumber, query.PageSize, cancellationToken);

        var tasks = items.Select(async project =>
        {
            var team = await GetTeamAsync(project.TenantId);
            var projectProgressDto = await taskServiceClient.GetProgresData(project.TenantId, project.Id);
            var progress = CalculateProgress(projectProgressDto);
            return MapToDto(project, team, progress);
        });

        var projectDtos = await Task.WhenAll(tasks);

        var response = new PaginatedResponse
        {
            PageNumber = query.PageNumber,
            PageSize = query.PageSize,
            TotalItems = totalCount,
            Items = projectDtos
        };

        await cacheService.SetAsync(PAGINATED_RESPONSE_KEY, response, cancellationToken, TimeSpan.FromHours(1));

        return new GetProjectsResult(response);
    }

    //TODO: to be reviewed
    private async Task<IEnumerable<UserDto>> GetTeamAsync(Guid teanantId)
    {
        var data = await userServiceClient.GetUsersByTenantIdAsync(teanantId);

        return data.Take(1);
    }

    private ProjectDto MapToDto(Models.Project project, IEnumerable<UserDto> team, int progress) => new()
    {
        Id = project.Id,
        Name = project.Name,
        Description = project.Description,
        CreatedAt = project.CreatedAt,
        CreatedBy = project.CreatedBy,
        TenantId = project.TenantId,
        ProjectStatus = project.ProjectStatus,
        StartDate = project.StartDate,
        EndDate = project.EndDate,
        Team = team,
        Progress = progress
    };

    //Simple progress calculation based on task statuses
    // TODO: Refine this logic as per actual requirements
    private int CalculateProgress(ProjectProgressDto progressDto)
    {
        // Protect against division by zero
        double taskProgress = progressDto.TotalTasks > 0
            ? (double)progressDto.CompletedTasks / progressDto.TotalTasks
            : 0.0;

        double subtaskProgress = progressDto.TotalSubtasks > 0
            ? (double)progressDto.CompletedSubtasks / progressDto.TotalSubtasks
            : 0.0;

        // Combine them with equal weight (optional)
        double totalProgress = (taskProgress + subtaskProgress) / 2.0;

        return (int)(totalProgress * 100);
    }
}
