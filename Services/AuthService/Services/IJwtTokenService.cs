using AuthService.Models;

namespace AuthService.Services;

public interface IJwtTokenService
{
    string GenerateToken(User user, string role, Guid tenantId);
    RefreshToken GenerateRefreshToken(Guid tenantId, Guid userId);
}