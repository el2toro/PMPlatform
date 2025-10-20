namespace TaskService.Application.Tasks.EventHandlers;

public class TaskDeletedEventHandler(ILogger<TaskDeletedEventHandler> logger, ITaskServiceHub taskServiceHub)
    : IConsumer<TaskDeletedEvent>
{
    public async Task Consume(ConsumeContext<TaskDeletedEvent> context)
    {
        logger.LogInformation("Task Event Handler: {TaskCreatedEvent}", context.Message.GetType().Name);

        Guid taskId = context.Message.Id;
        await taskServiceHub.SendTaskDeleted(taskId);
    }
}
