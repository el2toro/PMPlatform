namespace TaskService.Infrastructure.Repositories;

public class TaskServiceRepository(TaskServiceDbContext dbContext) : ITaskServiceRepository
{
    private readonly TaskServiceDbContext _dbContext = dbContext;
    public async Task<TaskItem> CreateTaskAsync(TaskItem task, CancellationToken cancellationToken)
    {
        //TODO: task cannot be created with subtasks, comment, attachement it throws an error

        var createdTask = _dbContext.Tasks.Add(task).Entity;
        await _dbContext.SaveChangesAsync(cancellationToken);

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
            .ToListAsync();
    }

    public async Task<TaskItem> GetTaskByIdAsync(Guid taskId, CancellationToken cancellationToken)
    {
        var task = await _dbContext.Tasks
           .Include(t => t.Subtasks)
           .Include(t => t.Comments)
           .Include(t => t.Attachments)
           .AsNoTracking()
           .FirstOrDefaultAsync(t => t.Id == taskId);

        ArgumentNullException.ThrowIfNull(nameof(task));

        return task;
    }

    public async Task<TaskItem> UpdateTaskAsync(TaskItem task, CancellationToken cancellationToken)
    {
        var existingTask = await _dbContext.Tasks
            .Include(t => t.Subtasks)
            .Include(t => t.Comments)
            .Include(t => t.Attachments)
            .FirstOrDefaultAsync(t => t.Id == task.Id);

        ArgumentNullException.ThrowIfNull(existingTask, nameof(existingTask));

        existingTask.Title = task.Title;
        existingTask.Description = task.Description;
        existingTask.DueDate = task.DueDate;
        existingTask.TaskStatus = task.TaskStatus;
        existingTask.AssignedTo = task.AssignedTo;
        existingTask.UpdatedAt = DateTime.UtcNow;
        existingTask.UpdatedBy = task.UpdatedBy;
        existingTask.Subtasks = task.Subtasks;
        existingTask.Comments = task.Comments;

        _dbContext.Tasks.Update(existingTask);
        await _dbContext.SaveChangesAsync();

        return existingTask;
    }

    public async Task<TaskItem> UpdateTaskStatusAsync(Guid taskId, Guid columnId, TaskItemStatus status, CancellationToken cancellationToken)
    {
        var task = await _dbContext.Tasks.FindAsync(taskId);

        ArgumentNullException.ThrowIfNull(task, nameof(task));

        task.TaskStatus = status;
        task.UpdatedAt = DateTime.UtcNow;
        _dbContext.Tasks.Update(task);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return task;
    }

    public async Task DeleteTaskAsync(Guid taskId, CancellationToken cancellationToken)
    {
        var task = await _dbContext.Tasks
           .Include(t => t.Subtasks)
           .Include(t => t.Comments)
           .Include(t => t.Attachments)
           .FirstOrDefaultAsync(t => t.Id == taskId);

        ArgumentNullException.ThrowIfNull(nameof(taskId));

        _dbContext.Tasks.Remove(task);
        await _dbContext.SaveChangesAsync();
    }
}
