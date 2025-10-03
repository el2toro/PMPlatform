using Mapster;
using MediatR;
using TaskService.Application.Dtos;
using TaskService.Domain.Entities;
using TaskService.Domain.Interfaces;

namespace TaskService.Application.Tasks.Commands.UpdateTask;

public record UpdateTaskCommand(TaskItemDto Task) : IRequest<UpdateTaskResult>;
public record UpdateTaskResult(TaskItemDto Task);

public class UpdateTaskHandler(ITaskServiceRepository taskServiceRepository)
    : IRequestHandler<UpdateTaskCommand, UpdateTaskResult>
{
    public async Task<UpdateTaskResult> Handle(UpdateTaskCommand command, CancellationToken cancellationToken)
    {
        var taskItem = command.Task.Adapt<TaskItem>();
        var updatedTask = await taskServiceRepository.UpdateTaskAsync(taskItem, cancellationToken);
        var result = updatedTask.Adapt<TaskItemDto>();

        return new UpdateTaskResult(result);
    }
}
