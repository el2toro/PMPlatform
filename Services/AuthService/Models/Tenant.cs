using AuthService.Enums;

namespace AuthService.Models;

public class Tenant
{
    public Guid TenantId { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = string.Empty;
    public string? Domain { get; set; } // optional (acme.com, etc.)
    public TenantPlan Plan { get; set; } = TenantPlan.Free;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    // Navigation
    public ICollection<UserTenant> UserTenants { get; set; } = new List<UserTenant>();
}
