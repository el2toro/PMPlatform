using Mapster;
using MediatR;
using Project.API.Dtos;
using Project.API.Enums;
using Project.API.Repository;

namespace Project.API.Project.Task.UpdateTaskStatus;

public record UpdateTaskStatusCommand(Guid TaskId, Guid ColumnId, TaskItemStatus Status) : IRequest<UpdateTaskStatusResponse>;
public record UpdateTaskStatusResponse(TaskItemDto TaskDto);

public class UpdateTaskStatusHandler(ITaskRepository taskRepository)
    : IRequestHandler<UpdateTaskStatusCommand, UpdateTaskStatusResponse>
{
    public async Task<UpdateTaskStatusResponse> Handle(UpdateTaskStatusCommand command, CancellationToken cancellationToken)
    {
        var updatedTask = await taskRepository.UpdateTaskStatusAsync(command.TaskId, command.ColumnId, command.Status, cancellationToken);
        var response = updatedTask.Adapt<TaskItemDto>();
        return new UpdateTaskStatusResponse(response);
    }
}
