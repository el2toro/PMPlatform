using Mapster;
using MediatR;
using Project.API.Dtos;
using Project.API.Models;
using Project.API.Repository;

namespace Project.API.Project.Task.CreateTask;

public record CreateTaskCommand(TaskItemDto Task) : IRequest<CreateTaskResult>;
public record CreateTaskResult(TaskItemDto Task);

public class CreateTaskHandler(ITaskRepository taskRepository)
    : IRequestHandler<CreateTaskCommand, CreateTaskResult>
{
    public async Task<CreateTaskResult> Handle(CreateTaskCommand command, CancellationToken cancellationToken)
    {
        var taskToCreate = command.Task.Adapt<TaskItem>();
        taskToCreate.CreatedAt = DateTime.UtcNow;
        //TODO: Replace with actual logged in user
        taskToCreate.CreatedBy = Guid.Parse("2ED46488-5667-42E6-A3B1-0046AB4BFCA6");
        taskToCreate.UpdatedBy = Guid.Parse("2ED46488-5667-42E6-A3B1-0046AB4BFCA6");
        var createdTask = await taskRepository.CreateTaskAsync(taskToCreate, cancellationToken);
        var result = createdTask.Adapt<TaskItemDto>();

        return new CreateTaskResult(result);
    }
}
