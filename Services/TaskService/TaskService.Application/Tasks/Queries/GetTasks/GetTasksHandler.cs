using Core.Services;

namespace TaskService.Application.Tasks.Queries.GetTasks;

public record GetTasksQuery(Guid ProjectId) : IRequest<GetTasksResult>;
public record GetTasksResult(IEnumerable<TaskItemDto> Tasks);

public class GetTasksHandler(ITaskServiceRepository taskServiceRepository,
    ICacheService cacheService)
    : IRequestHandler<GetTasksQuery, GetTasksResult>
{
    public async Task<GetTasksResult> Handle(GetTasksQuery query, CancellationToken cancellationToken)
    {
        //const string CACHED_TASKS_KEY = "tasks";

        //var cachedTasks = await cacheService.GetAsync<IEnumerable<TaskItemDto>>(CACHED_TASKS_KEY, cancellationToken);
        //if (cachedTasks is not null)
        //{
        //    return new GetTasksResult(cachedTasks);
        //}

        var taskItems = await taskServiceRepository.GetTasksAsync(query.ProjectId, cancellationToken);
        var result = taskItems.Adapt<IEnumerable<TaskItemDto>>();

        // await cacheService.SetAsync<IEnumerable<TaskItemDto>>(CACHED_TASKS_KEY, result, cancellationToken, TimeSpan.FromHours(1));

        return new GetTasksResult(result);
    }
}
