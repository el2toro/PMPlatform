namespace Core.Messaging.Events.Task;

public record TaskAssigneeChangedEvent
{
    public Guid Id { get; set; }
    public Guid AssignedTo { get; set; }
}
