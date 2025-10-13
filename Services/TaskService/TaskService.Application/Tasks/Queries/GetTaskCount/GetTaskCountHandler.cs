using Core.CQRS;

namespace TaskService.Application.Tasks.Queries.GetTaskCount;

public record GetTaskCountQuery(Guid ProjectId) : IQuery<GetTaskCountResult>;
public record GetTaskCountResult(TaskCountDto TaskCount);
public class GetTaskCountHandler(ITaskServiceRepository taskServiceRepository)
    : IQueryHandler<GetTaskCountQuery, GetTaskCountResult>
{
    public async Task<GetTaskCountResult> Handle(GetTaskCountQuery query, CancellationToken cancellationToken)
    {
        var data = await taskServiceRepository.GetTaskCountAsync(query.ProjectId, cancellationToken);
        TaskCountDto taskCountDto = new()
        {
            TotalTasks = data.totalTasks,
            CompletedTasks = data.completedTasks,
            TotalSubtasks = data.totalSubtasks,
            CompletedSubtasks = data.completedSubtasks,
        };
        return new GetTaskCountResult(taskCountDto);
    }
}
