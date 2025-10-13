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
            // var projectProgressDto = await projectRepository.GetProjectProgressAsync("78025c9d-f872-413d-8f44-0d3178cc71a5");
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

        // if (tasks.Count() == 0) return 0;

        // if (tasks.All(t => t.TaskStatus == TaskItemStatus.Done)) return 100;

        //  tasks = tasks.Where(t => t.TaskStatus != TaskItemStatus.Cancelled);

        //  int totalTasks = tasks.Count();
        //  double sumTaskProgress = 0;


        //    int totalSubtasks = task.Subtasks.Count();
        //    int completedSubtasks = task.Subtasks.Count(st => st.IsCompleted);

        double subtaskProgress =
              ((double)progressDto.CompletedSubtasks / progressDto.TotalSubtasks);
        //        : (task.TaskStatus == TaskItemStatus.Done ? 1 : 0);

        //    sumTaskProgress += taskProgress;



        return progressDto.TotalTasks > 0
            ? (int)(100 * ((progressDto.CompletedTasks / progressDto.TotalTasks) + subtaskProgress))
            : 0;
    }
}
