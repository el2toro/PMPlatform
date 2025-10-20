namespace TaskService.Application.Tasks.Commands.CreateTask;

public record CreateTaskCommand(TaskItemDto Task) : IRequest<CreateTaskResult>;
public record CreateTaskResult(TaskItemDto Task);

public class CreateTaskHandler(ITaskServiceRepository taskServiceRepository,
    TenantAwareContextService tenantAwareContextService,
    IPublishEndpoint publishEndpoint)
    : IRequestHandler<CreateTaskCommand, CreateTaskResult>
{
    public async Task<CreateTaskResult> Handle(CreateTaskCommand command, CancellationToken cancellationToken)
    {
        var taskItem = command.Task.Adapt<TaskItem>();
        Guid userId = tenantAwareContextService.UserId;

        taskItem.CreatedAt = DateTime.UtcNow;
        taskItem.UpdatedAt = DateTime.UtcNow;
        taskItem.CreatedBy = userId;
        taskItem.UpdatedBy = userId;

        if (taskItem is not null && taskItem.Comments?.Count > 0)
        {
            foreach (var comment in taskItem.Comments)
            {
                comment.CreatedAt = DateTime.UtcNow;
                comment.UpdatedAt = DateTime.UtcNow;
                comment.CommentedBy = userId;
            }
        }

        var createdTask = await taskServiceRepository.CreateTaskAsync(taskItem!, cancellationToken);
        var result = createdTask.Adapt<TaskItemDto>();

        await publishEndpoint.Publish<TaskCreatedEvent > (result);

        return new CreateTaskResult(result);
    }
}
