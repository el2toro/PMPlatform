using Mapster;
using MediatR;
using TaskService.Application.Dtos;
using TaskService.Domain.Entities;
using TaskService.Domain.Interfaces;

namespace TaskService.Application.Tasks.Commands.CreateTask;

public record CreateTaskCommand(TaskItemDto TaskItem) : IRequest<CreateTaskResult>;
public record CreateTaskResult(TaskItemDto TaskItem);

public class CreateTaskHandler(ITaskServiceRepository taskServiceRepository)
    : IRequestHandler<CreateTaskCommand, CreateTaskResult>
{
    public async Task<CreateTaskResult> Handle(CreateTaskCommand command, CancellationToken cancellationToken)
    {
        var taskItem = command.TaskItem.Adapt<TaskItem>();

        foreach (var comment in taskItem.Comments)
        {
            comment.CreatedAt = DateTime.UtcNow;
            comment.UpdatedAt = DateTime.UtcNow;
        }

        var createdTask = await taskServiceRepository.CreateTaskAsync(taskItem, cancellationToken);
        var result = createdTask.Adapt<TaskItemDto>();

        return new CreateTaskResult(result);
    }
}
