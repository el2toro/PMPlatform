namespace TaskService.Application.Tasks.EventHandlers;

public class TaskStatusChangeEventHandler
    (ILogger<TaskStatusChangeEventHandler> logger, ITaskServiceHub taskServiceHub)
    : IConsumer<TaskStatusChangedEvent>
{
    public async Task Consume(ConsumeContext<TaskStatusChangedEvent> context)
    {
        logger.LogInformation("Task Event Handler: {TaskStatusChangedEvent}", context.Message.GetType().Name);

        var taskStatusChangeEvent = context.Message;

        //await taskServiceHub.SendTaskStatus(taskStatusChangeEvent.Id, taskStatusChangeEvent.TaskStatus);
    }
}
