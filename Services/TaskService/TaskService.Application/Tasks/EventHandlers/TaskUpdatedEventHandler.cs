namespace TaskService.Application.Tasks.EventHandlers;

public class TaskUpdatedEventHandler(ILogger<TaskUpdatedEventHandler> logger, ITaskServiceHub taskServiceHub)
    : IConsumer<TaskUpdatedEvent>
{
    public async Task Consume(ConsumeContext<TaskUpdatedEvent> context)
    {
        logger.LogInformation("Task Event Handler: {TaskUpdatedEvent}", context.Message.GetType().Name);
        var task = context.Message.Adapt<TaskItemDto>();
        await taskServiceHub.SendUpdatedTask(task);
    }
}
