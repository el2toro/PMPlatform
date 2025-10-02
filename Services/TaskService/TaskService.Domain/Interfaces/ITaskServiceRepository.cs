using TaskService.Domain.Entities;
using TaskService.Domain.Enums;

namespace TaskService.Domain.Interfaces;

public interface ITaskServiceRepository
{
    Task<TaskItem> CreateTaskAsync(TaskItem task, CancellationToken cancellationToken);
    Task<IEnumerable<TaskItem>> GeTasksAsync(Guid projectId, CancellationToken cancellationToken);
    Task<TaskItem> GetTaskByIdAsync(Guid taskId, CancellationToken cancellationToken);
    Task<TaskItem> UpdateTaskAsync(TaskItem taskDto, CancellationToken cancellationToken);
    Task<TaskItem> UpdateTaskStatusAsync(Guid taskId, Guid columnId, TaskItemStatus status, CancellationToken cancellationToken);
}
