using Tenant.API.Enum;

namespace Tenant.API.Models;

public class UserTenant
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid UserId { get; set; }      // Comes from Auth Service
    public Guid TenantId { get; set; }    // FK to Tenant in this service

    public TenantRole Role { get; set; } = TenantRole.Member;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public Tenant Tenant { get; set; } = default!;
}
