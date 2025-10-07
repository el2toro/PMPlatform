namespace Core.Messaging.Events;

public class EventBase<T> where T : class
{
    public Guid EventId { get; set; } = Guid.NewGuid();
    public DateTime OccurredAt { get; set; } = DateTime.Now;
    public string EventType { get; set; } = default!;
    public string Origin { get; set; } = default!;
    public Guid TenantId { get; set; } = default!;
    public T EventData { get; set; } = default!;
}
