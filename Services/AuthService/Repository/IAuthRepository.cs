using AuthService.Dtos;

namespace AuthService.Repository;

public interface IAuthRepository
{
    Task RegisterUser(RegisterRequest request);
    Task<AuthResponse> Login(AuthRequest request);
    Task<AuthResponse> Logout(Guid userId);
    Task<AuthResponse> RefreshTokenAsync(string token, Guid tenantId);
    Task RevokeTokenAsync(string token, Guid tenantId);
}
