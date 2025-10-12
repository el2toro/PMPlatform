using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace TaskService.Application.Tasks.Queries.GetTasks;

public record GetTasksQuery(Guid ProjectId) : IRequest<GetTasksResult>;
public record GetTasksResult(IEnumerable<TaskItemDto> Tasks);

public class GetTasksHandler(ITaskServiceRepository taskServiceRepository, IDistributedCache distributedCache)
    : IRequestHandler<GetTasksQuery, GetTasksResult>
{
    public async Task<GetTasksResult> Handle(GetTasksQuery query, CancellationToken cancellationToken)
    {
        var cachedTasksString = await distributedCache.GetStringAsync("tasks");
        if (!string.IsNullOrEmpty(cachedTasksString))
        {
            var tasks = JsonSerializer.Deserialize<IEnumerable<TaskItemDto>>(cachedTasksString);
            return new GetTasksResult(tasks!);
        }

        var taskItems = await taskServiceRepository.GetTasksAsync(query.ProjectId, cancellationToken);
        var result = taskItems.Adapt<IEnumerable<TaskItemDto>>();

        var serializedResult = JsonSerializer.Serialize(result);
        await distributedCache.SetStringAsync("tasks", serializedResult);

        return new GetTasksResult(result);
    }
}
