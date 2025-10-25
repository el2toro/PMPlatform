namespace TaskService.Application.Tasks.Queries.GetAllTasks;

public record GetAllTasksQuery(Guid TenantId) : IQuery<GetAllTasksResult>;
public record GetAllTasksResult(IEnumerable<TaskItemDto> Tasks);

internal class GetAllTasksHandler(ITaskServiceRepository taskServiceRepository)
    : IQueryHandler<GetAllTasksQuery, GetAllTasksResult>
{
    public async Task<GetAllTasksResult> Handle(GetAllTasksQuery query, CancellationToken cancellationToken)
    {
        var tasks = await taskServiceRepository.GetAllTasksAsync(query.TenantId, cancellationToken);
        var result = tasks.Adapt<IEnumerable<TaskItemDto>>();

        return new GetAllTasksResult(result);
    }
}
