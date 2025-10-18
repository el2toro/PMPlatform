namespace TaskService.Application.Tasks.EventHandlers;

public class TaskAssigneeChangedEvendHandler
    (ILogger<TaskAssigneeChangedEvendHandler> logger, ITaskServiceHub taskServiceHub)
    : IConsumer<TaskAssigneeChangedEvent>
{
    public async Task Consume(ConsumeContext<TaskAssigneeChangedEvent> context)
    {
        logger.LogInformation("Task Event Handler: {TaskAssigneeChangedEvent}", context.Message.GetType().Name);
        var taskDto = context.Message.Adapt<TaskItemDto>();

        await taskServiceHub.SendTaskAssignee(taskDto);
    }
}
