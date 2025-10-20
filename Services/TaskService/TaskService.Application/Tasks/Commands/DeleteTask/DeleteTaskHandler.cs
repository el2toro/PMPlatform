namespace TaskService.Application.Tasks.Commands.DeleteTask;

public record DeleteTaskCommand(Guid projectId, Guid TaskId) : ICommand<DeleteTaskResult>;
public record DeleteTaskResult(bool IsSuccess);

public class DeleteTaskHandler(ITaskServiceRepository taskServiceRepository,
    IPublishEndpoint publishEndpoint)
    : ICommandHandler<DeleteTaskCommand, DeleteTaskResult>
{
    public async Task<DeleteTaskResult> Handle(DeleteTaskCommand command, CancellationToken cancellationToken)
    {
        var existingTask = await taskServiceRepository
            .GetTaskByIdAsync(command.projectId, command.TaskId, cancellationToken)
            ?? throw new TaskNotFoundException(command.TaskId.ToString());

        await taskServiceRepository.DeleteTaskAsync(existingTask, cancellationToken);

        //Notify about deleted task
        await publishEndpoint.Publish(new TaskDeletedEvent(existingTask.Id), cancellationToken);

        return new DeleteTaskResult(true);
    }
}
