using Mapster;

namespace TaskService.Infrastructure.Repositories;

public class TaskServiceRepository(TaskServiceDbContext dbContext) : ITaskServiceRepository
{
    private readonly TaskServiceDbContext _dbContext = dbContext;
    public async Task<TaskItem> CreateTaskAsync(TaskItem task, CancellationToken cancellationToken)
    {
        //TODO: task cannot be created with subtasks, comment, attachement it throws an error

        var createdTask = _dbContext.Tasks.Add(task).Entity;
        await _dbContext.SaveChangesAsync();

        return createdTask;
    }

    public async Task<IEnumerable<TaskItem>> GetTasksAsync(Guid projectId, CancellationToken cancellationToken)
    {
        return await _dbContext.Tasks
            .Include(t => t.Subtasks)
            .Include(t => t.Comments)
            .Include(t => t.Attachments)
            .AsNoTracking()
            .Where(t => t.ProjectId == projectId)
            .ToListAsync(cancellationToken);
    }

    public async Task<TaskItem> GetTaskByIdAsync(Guid projectId, Guid taskId, CancellationToken cancellationToken)
    {
        var task = await _dbContext.Tasks
           .Include(t => t.Subtasks)
           .Include(t => t.Comments)
           .Include(t => t.Attachments)
           .AsNoTracking()
           .FirstOrDefaultAsync(t => t.ProjectId == projectId && t.Id == taskId, cancellationToken);

        return task;
    }

    public async Task<TaskItem> UpdateTaskAsync(TaskItem task, CancellationToken cancellationToken)
    {

        var updatedTask = _dbContext.Tasks.Update(task).Entity;
        await _dbContext.SaveChangesAsync(cancellationToken);

        return updatedTask;
    }

    public async Task<TaskItem> UpdateTaskStatusAsync(Guid projectId, Guid taskId, TaskItemStatus status, CancellationToken cancellationToken)
    {
        var task = await _dbContext.Tasks.FirstOrDefaultAsync(t => t.ProjectId == projectId && t.Id == taskId);

        task.TaskStatus = status;
        task.UpdatedAt = DateTime.UtcNow;
        // _dbContext.Tasks.Update(task);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return task;
    }

    public async Task DeleteTaskAsync(Guid taskId, CancellationToken cancellationToken)
    {
        var task = await _dbContext.Tasks
           .Include(t => t.Subtasks)
           .Include(t => t.Comments)
           .Include(t => t.Attachments)
           .FirstOrDefaultAsync(t => t.Id == taskId, cancellationToken);

        ArgumentNullException.ThrowIfNull(nameof(taskId));

        _dbContext.Tasks.Remove(task);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<(int totalTasks, int completedTasks, int totalSubtasks, int completedSubtasks)>
        GetTaskCountAsync(Guid projectId, CancellationToken cancellationToken)
    {
        var taskIds = await _dbContext.Tasks
            .Where(task => task.ProjectId == projectId)
            .Select(t => t.Id)
            .ToListAsync();

        int completedTasks = await _dbContext.Tasks
            .Where(task => task.ProjectId == projectId && task.TaskStatus == TaskItemStatus.Done)
            .CountAsync();

        var totalSubtasks = await _dbContext.Subtasks
            .Where(subtask => taskIds.Contains(subtask.TaskId))
            .CountAsync();

        var completedSubtasks = await _dbContext.Subtasks
            .Where(subtask => taskIds.Contains(subtask.TaskId) && subtask.IsCompleted)
            .CountAsync();

        return (taskIds.Count, completedTasks, totalSubtasks, completedSubtasks);
    }
}
