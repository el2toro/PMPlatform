using Core.CQRS;
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
   IDistributedCache distributedCache,
    UserServiceClient userServiceClient)
    : IQueryHandler<GetProjectsQuery, GetProjectsResponse>
{
    public async Task<GetProjectsResponse> Handle(GetProjectsQuery query, CancellationToken cancellationToken)
    {
        var dataString = await distributedCache.GetStringAsync("projects");
        if (!string.IsNullOrEmpty(dataString))
        {
            var responseData = JsonSerializer.Deserialize<PaginatedResponse>(dataString);
            return new GetProjectsResponse(responseData);
        }

        var (items, totalCount) = await projectRepository.GetProjectsAsync(query.TenantId, query.PageNumber, query.PageSize, cancellationToken);

        var safeDtosCollection = new ConcurrentBag<ProjectDto>();

        var tasks = items.Select(project =>
        // GetTeam runs in parallel
        GetTeam(project.TenantId)
        .ContinueWith(taskTeam =>
        {
            var team = taskTeam.Result;
            var projectDtos = MapToDto(project, team);

            safeDtosCollection.Add(projectDtos);
        }));

        await Task.WhenAll(tasks);

        var response = new PaginatedResponse
        {
            PageNumber = query.PageNumber,
            PageSize = query.PageSize,
            TotalItems = totalCount,
            Items = safeDtosCollection
        };

        var data = JsonSerializer.Serialize(response);

        await distributedCache.SetStringAsync("projects", data);

        return new GetProjectsResponse(response);
    }

    private async Task<IEnumerable<UserDto>> GetTeam(Guid teanantId)
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
