using Project.API.Enums;
using Project.API.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Project.API.Repository;

public interface ITaskRepository
{
    Task<TaskItem> CreateTaskAsync(TaskItem task, CancellationToken cancellationToken);
    Task<IEnumerable<TaskItem>> GeTasksAsync(Guid projectId, CancellationToken cancellationToken);
    Task<TaskItem> GetTaskByIdAsync(Guid taskId, CancellationToken cancellationToken);
    Task<TaskItem> UpdateTaskAsync(TaskItem taskDto, CancellationToken cancellationToken);
    Task<TaskItem> UpdateTaskStatusAsync(Guid taskId, Guid columnId, TaskItemStatus status, CancellationToken cancellationToken);
    Task<IEnumerable<TaskItem>> GeTasksByColumnIdAsync(Guid columnId, CancellationToken cancellationToken);
}
