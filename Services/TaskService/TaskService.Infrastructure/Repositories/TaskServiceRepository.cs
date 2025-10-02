using Microsoft.EntityFrameworkCore;
using TaskService.Domain.Entities;
using TaskService.Domain.Enums;
using TaskService.Domain.Interfaces;
using TaskService.Infrastructure.Persistance;

namespace TaskService.Infrastructure.Repositories;

public class TaskServiceRepository(TaskServiceDbContext dbContext) : ITaskServiceRepository
{
    private readonly TaskServiceDbContext _dbContext = dbContext;
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
        task.UpdatedAt = DateTime.UtcNow;
        _dbContext.Tasks.Update(task);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return task;
    }
}
