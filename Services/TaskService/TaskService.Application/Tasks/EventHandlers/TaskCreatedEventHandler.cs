namespace TaskService.Application.Tasks.EventHandlers;

public class TaskCreatedEventHandler(ILogger<TaskCreatedEventHandler> logger, ITaskServiceHub taskServiceHub)
    : IConsumer<TaskCreatedEvent>
{
    public async Task Consume(ConsumeContext<TaskCreatedEvent> context)
    {
        logger.LogInformation("Task Event Handler: {TaskCreatedEvent}", context.Message.GetType().Name);

        var task = context.Message.Adapt<TaskItemDto>();

        await taskServiceHub.SendCreatedTask(task);
    }
}
