using Microsoft.AspNetCore.Http;

namespace Core.Services;

public class TenantAwareContextService(IHttpContextAccessor httpContextAccessor)
{
    public Guid UserId => Guid.TryParse(httpContextAccessor.HttpContext?.Request.Headers["X-User-Id"], out Guid userId)
               ? userId
               : throw new UnauthorizedAccessException("Invalid or missing User");

    public Guid TenantId => Guid.TryParse(httpContextAccessor.HttpContext?.Request.Headers["X-Tenant-Id"], out Guid tenantId)
              ? tenantId
              : throw new UnauthorizedAccessException("Invalid or missing TenantId");
}
