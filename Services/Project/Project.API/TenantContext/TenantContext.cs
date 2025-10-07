namespace Project.API.TenantContext;

public interface ITenantContext
{
    Guid UserId { get; }
    Guid TenantId { get; }
}

public class TenantContext(IHttpContextAccessor accessor) : ITenantContext
{
    public Guid UserId { get; } = Guid.TryParse(accessor.HttpContext?.Request.Headers["X-User-Id"], out Guid userId)
            ? userId
            : throw new UnauthorizedAccessException("Invalid or missing User");
    public Guid TenantId { get; } = Guid.TryParse(accessor.HttpContext?.Request.Headers["X-Tenant-Id"], out Guid tenantId)
            ? tenantId
            : throw new UnauthorizedAccessException("Invalid or missing TenantId");
}
