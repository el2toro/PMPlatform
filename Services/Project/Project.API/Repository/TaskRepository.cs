using Microsoft.EntityFrameworkCore;
using Project.API.Data;
using Project.API.Enums;
using Project.API.Models;

namespace Project.API.Repository;

public class TaskRepository(ProjectDbContext dbContext) : ITaskRepository
{
    private readonly ProjectDbContext _dbContext = dbContext;

    public async Task<TaskItem> CreateTaskAsync(TaskItem task, CancellationToken cancellationToken)
    {
        var createdTask = _dbContext.Tasks.Add(task).Entity;
        await _dbContext.SaveChangesAsync(cancellationToken);

        return createdTask;
    }

    public Task<IEnumerable<TaskItem>> GeTasksAsync(Guid projectId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<TaskItem> GetTaskByIdAsync(Guid taskId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<TaskItem> UpdateTaskAsync(TaskItem task, CancellationToken cancellationToken)
    {
        var existingTask = await _dbContext.Tasks
            .Include(t => t.Subtasks)
            .Include(t => t.Comments)
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
        task.ColumnId = columnId;
        task.UpdatedAt = DateTime.UtcNow;
        _dbContext.Tasks.Update(task);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return task;
    }

    public async Task<IEnumerable<TaskItem>> GeTasksByColumnIdAsync(Guid columnId, CancellationToken cancellationToken)
    {
        var tasks = await _dbContext.Tasks
            .AsNoTracking()
            .Where(t => t.ColumnId == columnId)
            .ToListAsync(cancellationToken);

        return tasks;
    }
}
