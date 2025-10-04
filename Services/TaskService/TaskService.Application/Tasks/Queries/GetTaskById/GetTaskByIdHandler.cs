namespace TaskService.Application.Tasks.Queries.GetTaskById;

public record GetTaskByIdQuery(Guid ProjectId, Guid TaskId) : IRequest<GetTaskByIdResult>;
public record GetTaskByIdResult(TaskItemDto Task);

public class GetTaskByIdHandler(ITaskServiceRepository taskServiceRepository)
    : IRequestHandler<GetTaskByIdQuery, GetTaskByIdResult>
{
    public async Task<GetTaskByIdResult> Handle(GetTaskByIdQuery query, CancellationToken cancellationToken)
    {
        var task = await taskServiceRepository.GetTaskByIdAsync(query.ProjectId, query.TaskId, cancellationToken);
        var result = task.Adapt<TaskItemDto>();

        return new GetTaskByIdResult(result);
    }
}
