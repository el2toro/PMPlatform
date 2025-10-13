using Core.CQRS;
using Core.Services;
using FluentValidation;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections.Concurrent;
using System.Data;
using System.Text.Json;

namespace Project.API.Project.GetProjects;

public record GetProjectsQuery(Guid TenantId, int PageNumber, int PageSize) : IQuery<GetProjectsResponse>;
public record GetProjectsResponse(PaginatedResponse PaginatedResponse);

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
    UserServiceClient userServiceClient)
    : IQueryHandler<GetProjectsQuery, GetProjectsResponse>
{
    public async Task<GetProjectsResponse> Handle(GetProjectsQuery query, CancellationToken cancellationToken)
    {
        string PAGINATED_RESPONSE_KEY = $"paginated-response-{query.PageNumber}&{query.PageSize}";

        var cachedPaginatedResponse = await cacheService.GetAsync<PaginatedResponse>(PAGINATED_RESPONSE_KEY, cancellationToken);
        if (cachedPaginatedResponse is not null)
        {
            return new GetProjectsResponse(cachedPaginatedResponse);
        }

        var (items, totalCount) = await projectRepository
            .GetProjectsAsync(query.TenantId, query.PageNumber, query.PageSize, cancellationToken);

        var tasks = items.Select(async project =>
        {
            var team = await GetTeamAsync(project.TenantId);
            return MapToDto(project, team);
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

        return new GetProjectsResponse(response);
    }

    //TODO: to be reviewed
    private async Task<IEnumerable<UserDto>> GetTeamAsync(Guid teanantId)
    {
        var data = await userServiceClient.GetUsersByTenantIdAsync(teanantId);

        return data.Take(1);
    }

    private ProjectDto MapToDto(Models.Project project, IEnumerable<UserDto> team)
    {
        return new ProjectDto
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
            Team = team
        };
    }

    // Simple progress calculation based on task statuses
    // TODO: Refine this logic as per actual requirements
    //    private int CalculateProgress(IEnumerable<Models.TaskItem> tasks)
    //    {
    //        if (tasks.Count() == 0) return 0;

    //        if (tasks.All(t => t.TaskStatus == TaskItemStatus.Done)) return 100;

    //        tasks = tasks.Where(t => t.TaskStatus != TaskItemStatus.Cancelled);

    //        int totalTasks = tasks.Count();
    //        double sumTaskProgress = 0;

    //        foreach (var task in tasks)
    //        {
    //            int totalSubtasks = task.Subtasks.Count();
    //            int completedSubtasks = task.Subtasks.Count(st => st.IsCompleted);

    //            double taskProgress = totalSubtasks > 0
    //                ? ((double)completedSubtasks / totalSubtasks)
    //                : (task.TaskStatus == TaskItemStatus.Done ? 1 : 0);

    //            sumTaskProgress += taskProgress;
    //        }


    //        return totalTasks > 0
    //            ? (int)(100 * (sumTaskProgress / totalTasks))
    //            : 0;
    //    }
}
