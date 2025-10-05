using Mapster;
using MediatR;
using Project.API.Dtos;
using Project.API.Models;
using Project.API.Repository;

namespace Project.API.Project.Task.UpdateTask;

public record UpdateTaskCommand(TaskItemDto TaskDto) : IRequest<UpdateTaskResponse>;
public record UpdateTaskResponse(TaskItemDto TaskDto);
public class UpdateTaskHandler(ITaskRepository taskRepository)
    : IRequestHandler<UpdateTaskCommand, UpdateTaskResponse>
{
    public async Task<UpdateTaskResponse> Handle(UpdateTaskCommand command, CancellationToken cancellationToken)
    {
        var taskToUpdate = command.TaskDto.Adapt<TaskItem>();
        var updatedTask = await taskRepository.UpdateTaskAsync(taskToUpdate, cancellationToken);
        var result = updatedTask.Adapt<TaskItemDto>();
        return new UpdateTaskResponse(result);
    }
}
