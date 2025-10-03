namespace TaskService.Application.Tasks.Commands.DeleteTask;

public record DeleteTaskCommand(Guid TaskId) : IRequest<DeleteTaskResult>;
public record DeleteTaskResult(bool IsSuccess);

public class DeleteTaskHandler(ITaskServiceRepository taskServiceRepository)
    : IRequestHandler<DeleteTaskCommand, DeleteTaskResult>
{
    public async Task<DeleteTaskResult> Handle(DeleteTaskCommand command, CancellationToken cancellationToken)
    {
        await taskServiceRepository.DeleteTaskAsync(command.TaskId, cancellationToken);
        return new DeleteTaskResult(true);
    }
}
