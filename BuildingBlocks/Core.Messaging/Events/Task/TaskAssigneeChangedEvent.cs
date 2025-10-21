namespace Core.Messaging.Events.Task;

public record TaskAssigneeChangedEvent
{
    public Guid Id { get; set; }
    public string Title { get; set; } = default!;
    public Guid AssignedTo { get; set; }
}
