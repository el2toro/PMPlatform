using TaskService.Domain.Enums;

namespace TaskService.Application.Tasks.Commands.UpdateTaskStatus;


public record UpdateTaskStatusCommand(Guid ProjectId, Guid TaskId, TaskItemStatus Status) : IRequest<UpdateTaskStatusResponse>;
public record UpdateTaskStatusResponse(TaskItemDto Task);

public class UpdateTaskStatusHandler(ITaskServiceRepository taskServiceRepository)
    : IRequestHandler<UpdateTaskStatusCommand, UpdateTaskStatusResponse>
{
    public async Task<UpdateTaskStatusResponse> Handle(UpdateTaskStatusCommand command, CancellationToken cancellationToken)
    {
        var updatedTask = await taskServiceRepository.UpdateTaskStatusAsync(command.ProjectId, command.TaskId, command.Status, cancellationToken);
        var response = updatedTask.Adapt<TaskItemDto>();
        return new UpdateTaskStatusResponse(response);
    }
}
