namespace TaskService.Domain.Interfaces;

public interface ITaskServiceRepository
{
    Task<TaskItem> CreateTaskAsync(TaskItem task, CancellationToken cancellationToken);
    Task<IEnumerable<TaskItem>> GetTasksAsync(Guid projectId, CancellationToken cancellationToken);
    Task<TaskItem> GetTaskByIdAsync(Guid projectId, Guid taskId, CancellationToken cancellationToken);
    Task<TaskItem> UpdateTaskAsync(TaskItem taskDto, CancellationToken cancellationToken);
    Task<TaskItem> UpdateTaskStatusAsync(Guid projectId, Guid taskId, TaskItemStatus status, CancellationToken cancellationToken);
    Task DeleteTaskAsync(Guid taskId, CancellationToken cancellationToken);
}
