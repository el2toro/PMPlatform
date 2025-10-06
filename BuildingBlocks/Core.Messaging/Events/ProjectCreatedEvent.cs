namespace Core.Messaging.Events;

public class ProjectCreatedEvent
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = "This is ProjecCreatedEvent";
}
