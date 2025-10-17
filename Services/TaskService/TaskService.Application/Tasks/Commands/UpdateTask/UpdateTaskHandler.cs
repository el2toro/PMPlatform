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
        var existingTask = await taskServiceRepository.GetTaskByIdAsync(command.Task.ProjectId, command.Task.Id, cancellationToken);

        if (existingTask is null)
            throw new TaskNotFoundException(command.Task.Id.ToString());

        var taskItem = command.Task.Adapt<TaskItem>();

        foreach (var comment in taskItem.Comments)
        {
            //comment.CreatedAt = DateTime.UtcNow;
            comment.UpdatedAt = DateTime.UtcNow;
        }

        taskItem.UpdatedAt = DateTime.UtcNow;
        taskItem.UpdatedBy = tenantContext.UserId;

        var updatedTask = await taskServiceRepository.UpdateTaskAsync(taskItem, cancellationToken);
        var result = updatedTask.Adapt<TaskItemDto>();

        await publishEndpoint.Publish<TaskUpdatedEvent>(result);

        return new UpdateTaskResult(result);
    }
}
