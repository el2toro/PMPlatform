namespace AuthService.Models;
// -------------------
// User-Tenant Membership
// -------------------
public class UserTenant
{
    public Guid UserTenantId { get; set; } = Guid.NewGuid();

    public Guid UserId { get; set; }
    public Guid TenantId { get; set; }
    public TenantRole Role { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public User User { get; set; } = default!;
}
