using Project.API.Models;

namespace Project.API.Repository;

public interface ITaskRepository
{
    Task<TaskItem> CreateTaskAsync(TaskItem task, CancellationToken cancellationToken);
    Task<IEnumerable<TaskItem>> GeTasksAsync(Guid projectId, CancellationToken cancellationToken);
    Task<TaskItem> GetTaskByIdAsync(Guid taskId, CancellationToken cancellationToken);
    Task<TaskItem> UpdateTaskAsync(TaskItem taskDto, CancellationToken cancellationToken);
}
