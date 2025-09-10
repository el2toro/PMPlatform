namespace AuthService.Models;
// -------------------
// Refresh Token
// -------------------
public class RefreshToken
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Token { get; set; } = default!;
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? RevokedAt { get; set; }

    // Foreign keys
    public Guid UserId { get; set; }
    public User User { get; set; } = default!;

    public Guid TenantId { get; set; }
    public Tenant Tenant { get; set; } = default!;

    public bool IsActive => RevokedAt == null && DateTime.UtcNow <= ExpiresAt;
}



