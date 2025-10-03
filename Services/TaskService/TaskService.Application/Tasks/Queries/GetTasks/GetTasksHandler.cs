namespace TaskService.Application.Tasks.Queries.GetTasks;

public record GetTasksQuery(Guid ProjectId) : IRequest<GetTasksResult>;
public record GetTasksResult(IEnumerable<TaskItemDto> Tasks);

public class GetTasksHandler(ITaskServiceRepository taskServiceRepository)
    : IRequestHandler<GetTasksQuery, GetTasksResult>
{
    public async Task<GetTasksResult> Handle(GetTasksQuery query, CancellationToken cancellationToken)
    {
        var taskItems = await taskServiceRepository.GetTasksAsync(query.ProjectId, cancellationToken);
        var result = taskItems.Adapt<IEnumerable<TaskItemDto>>();

        return new GetTasksResult(result);
    }
}
