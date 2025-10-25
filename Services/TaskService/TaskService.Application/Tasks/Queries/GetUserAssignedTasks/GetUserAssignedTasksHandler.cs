namespace TaskService.Application.Tasks.Queries.GetUserAssignedTasks;

public record GetUserAssignedTasksQuery(Guid TenantId, Guid UserId) : IQuery<GetUserAssignedTasksResult>;
public record GetUserAssignedTasksResult(IEnumerable<TaskItemDto> Tasks);
internal class GetUserAssignedTasksHandler(ITaskServiceRepository taskServiceRepository)
    : IQueryHandler<GetUserAssignedTasksQuery, GetUserAssignedTasksResult>
{
    public async Task<GetUserAssignedTasksResult> Handle(GetUserAssignedTasksQuery query, CancellationToken cancellationToken)
    {
        var tasks = await taskServiceRepository.GetUserAssigendTasksAsync(query.TenantId, query.UserId, cancellationToken);
        var result = tasks.Adapt<IEnumerable<TaskItemDto>>();

        return new GetUserAssignedTasksResult(result);
    }
}
