using AuthService.Dtos;
using AuthService.Models;

namespace AuthService.Services;

public interface IJwtTokenService
{
    string GenerateToken(User user, string role, Guid tenantId);
    RefreshToken GenerateRefreshToken(Guid tenantId);
    Task<AuthResponse> RefreshTokenAsync();
    Task<AuthResponse> RevokeTokenAsync();
    bool ValidateToken(string token, out Guid userId, out string email, out IEnumerable<string> roles);
}