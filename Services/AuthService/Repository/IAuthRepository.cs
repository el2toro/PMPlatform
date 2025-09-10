using AuthService.Dtos;

namespace AuthService.Repository;

public interface IAuthRepository
{
    Task RegisterUser(RegisterRequest request);
    Task<AuthResponse> Login(string email, string password);
    Task Logout(string refreshToken, Guid tenantId);
    Task<AuthResponse> RefreshTokenAsync(string token, Guid tenantId);
}
