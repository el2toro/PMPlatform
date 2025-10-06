namespace TaskService.Application.Tasks.Commands.CreateTask;

public record CreateTaskCommand(TaskItemDto Task) : IRequest<CreateTaskResult>;
public record CreateTaskResult(TaskItemDto Task);

public class CreateTaskHandler(ITaskServiceRepository taskServiceRepository)
    : IRequestHandler<CreateTaskCommand, CreateTaskResult>
{
    public async Task<CreateTaskResult> Handle(CreateTaskCommand command, CancellationToken cancellationToken)
    {
        var taskItem = command.Task.Adapt<TaskItem>();

        taskItem.CreatedAt = DateTime.UtcNow;
        taskItem.UpdatedAt = DateTime.UtcNow;

        var createdTask = await taskServiceRepository.CreateTaskAsync(taskItem, cancellationToken);
        var result = createdTask.Adapt<TaskItemDto>();

        return new CreateTaskResult(result);
    }
}
