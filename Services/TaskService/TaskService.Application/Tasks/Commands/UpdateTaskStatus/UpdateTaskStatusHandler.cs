namespace TaskService.Application.Tasks.Commands.UpdateTaskStatus;


public record UpdateTaskStatusCommand(Guid ProjectId, Guid TaskId, Domain.Enums.TaskItemStatus Status) : IRequest<UpdateTaskStatusResult>;
public record UpdateTaskStatusResult(TaskItemDto Task);

public class UpdateTaskStatusHandler(ITaskServiceRepository taskServiceRepository, IPublishEndpoint publishEndpoint)
    : IRequestHandler<UpdateTaskStatusCommand, UpdateTaskStatusResult>
{
    public async Task<UpdateTaskStatusResult> Handle(UpdateTaskStatusCommand command, CancellationToken cancellationToken)
    {
        var updatedTask = await taskServiceRepository.UpdateTaskStatusAsync(command.ProjectId, command.TaskId, command.Status, cancellationToken);
        var result = updatedTask.Adapt<TaskItemDto>();

        await publishEndpoint.Publish<TaskUpdatedEvent>(result);

        return new UpdateTaskStatusResult(result);
    }
}
