using TaskService.Domain.Entities;

namespace TaskService.Application.Tasks.Commands.UpdateTask;

public record UpdateTaskCommand(TaskItemDto Task) : IRequest<UpdateTaskResult>;
public record UpdateTaskResult(TaskItemDto Task);

public class UpdateTaskHandler(ITaskServiceRepository taskServiceRepository,
    TenantAwareContextService tenantContext,
    IPublishEndpoint publishEndpoint)
    : IRequestHandler<UpdateTaskCommand, UpdateTaskResult>
{
    public async Task<UpdateTaskResult> Handle(UpdateTaskCommand command, CancellationToken cancellationToken)
    {
        var existingTask = await taskServiceRepository.GetTaskByIdAsync(command.Task.ProjectId, command.Task.Id, cancellationToken)
            ?? throw new TaskNotFoundException(command.Task.Id.ToString());

        existingTask = command.Task.Adapt<TaskItem>();

        var updatedTask = await taskServiceRepository.UpdateTaskAsync(existingTask, cancellationToken);
        var result = updatedTask.Adapt<TaskItemDto>();

        //Notify user on task assigned to them
        //TODO: notify assigned user not logged in user
        if (tenantContext.UserId == command.Task.AssignedTo)
            await publishEndpoint.Publish<TaskAssigneeChangedEvent>(result, cancellationToken);

        await publishEndpoint.Publish<TaskUpdatedEvent>(result);

        return new UpdateTaskResult(result);
    }
}
