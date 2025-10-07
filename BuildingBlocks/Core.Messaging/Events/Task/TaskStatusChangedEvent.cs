namespace Core.Messaging.Events.Task;

public record TaskStatusChangedEvent
{
    public Guid Id { get; set; }
    public int TaskStatus { get; set; }
}
