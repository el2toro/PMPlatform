namespace Core.Messaging.Events;

public class ProjectCreatedEvent
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;

}
