namespace TaskService.Application.Tasks.Commands.AssigneTask;

public record AssigneTaskCommand(Guid taskId, Guid AddignedTo) : IRequest<AssigneTaskResult>;
public record AssigneTaskResult();
public class AssigneTaskHandler(ITaskServiceRepository taskServiceRepository, IPublishEndpoint publishEndpoint)
    : IRequestHandler<AssigneTaskCommand, AssigneTaskResult>
{
    public async Task<AssigneTaskResult> Handle(AssigneTaskCommand command, CancellationToken cancellationToken)
    {
        // await taskServiceRepository.AssigneTask(command.taskId, command.AddignedTo, cancellationToken);
        // await publishEndpoint.Publish<TaskAssigneeChangedEvent>(cancellationToken);
        throw new NotImplementedException();
    }
}
