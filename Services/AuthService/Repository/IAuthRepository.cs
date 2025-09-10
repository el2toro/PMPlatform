using AuthService.Dtos;

namespace AuthService.Repository;

public interface IAuthRepository
{
    Task RegisterUser(RegisterRequestDto request, CancellationToken cancellationToken);
    Task<UserProfileDto> Login(string email, string password, CancellationToken cancellationToken);
    Task Logout(string refreshToken, Guid tenantId, CancellationToken cancellationToken);
    Task<UserProfileDto> RefreshTokenAsync(string refreshToken, Guid tenantId, CancellationToken cancellationToken);
}
