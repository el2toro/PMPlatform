using AuthService.Dtos;

namespace AuthService.Repository;

public interface IAuthRepository
{
    Task RegisterUser(RegisterRequest request, CancellationToken cancellationToken);
    Task<AuthResponse> Login(string email, string password, CancellationToken cancellationToken);
    Task Logout(string refreshToken, Guid tenantId, CancellationToken cancellationToken);
    Task<AuthResponse> RefreshTokenAsync(string refreshToken, Guid tenantId, CancellationToken cancellationToken);
}
