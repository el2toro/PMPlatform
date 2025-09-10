namespace Tenant.API.Models;

public class Tenant
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = default!;

    public string? Description { get; set; }

    public Guid OwnerId { get; set; }   // User who created/owns the tenant

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public ICollection<UserTenant> UserTenants { get; set; } = new List<UserTenant>();
}

