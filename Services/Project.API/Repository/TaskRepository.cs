using Project.API.Data;
using Project.API.Dtos;
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
        var existingTask = _dbContext.Tasks.Find(task.Id);

        ArgumentNullException.ThrowIfNull(existingTask, nameof(existingTask));

        existingTask.Title = task.Title;
        existingTask.Description = task.Description;
        existingTask.DueDate = task.DueDate;
        existingTask.TaskStatus = task.TaskStatus;
        existingTask.AssignedTo = task.AssignedTo;
        existingTask.UpdatedAt = DateTime.UtcNow;
        existingTask.UpdatedBy = task.UpdatedBy;

        //TODO: Add subtasks and comments mapping

        _dbContext.Tasks.Update(existingTask);
        await _dbContext.SaveChangesAsync();

        return existingTask;
    }
}
