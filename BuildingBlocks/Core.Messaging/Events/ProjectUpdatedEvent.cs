namespace Core.Messaging.Events;

public record ProjectUpdatedEvent
{
    public Guid Id { get; init; }
    public string Name { get; init; } = default!;
    public string? Description { get; init; }
    public DateTime CreatedAt { get; init; }
    public Guid CreatedBy { get; init; }
    public Guid TenantId { get; init; }
    public int ProjectStatus { get; init; }
    public DateTime StartDate { get; init; }
    public DateTime EndDate { get; init; }
    public int Progress { get; init; }
}
