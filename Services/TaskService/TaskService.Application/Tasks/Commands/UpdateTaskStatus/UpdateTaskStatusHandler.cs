using TaskService.Domain.Enums;

namespace TaskService.Application.Tasks.Commands.UpdateTaskStatus;


public record UpdateTaskStatusCommand(Guid ProjectId, Guid TaskId, TaskItemStatus Status) : IRequest<UpdateTaskStatusResult>;
public record UpdateTaskStatusResult(TaskItemDto Task);

public class UpdateTaskStatusHandler(ITaskServiceRepository taskServiceRepository, IPublishEndpoint publishEndpoint)
    : IRequestHandler<UpdateTaskStatusCommand, UpdateTaskStatusResult>
{
    public async Task<UpdateTaskStatusResult> Handle(UpdateTaskStatusCommand command, CancellationToken cancellationToken)
    {
        var updatedTask = await taskServiceRepository.UpdateTaskStatusAsync(command.ProjectId, command.TaskId, command.Status, cancellationToken);
        var result = updatedTask.Adapt<TaskItemDto>();

        //TODO: remove casting
        await publishEndpoint.Publish<TaskStatusChangedEvent>(new TaskStatusChangedEvent
        {
            Id = result.Id,
            TaskStatus = (int)result.TaskStatus
        });

        return new UpdateTaskStatusResult(result);
    }
}
