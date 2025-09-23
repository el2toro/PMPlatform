using Auth.API.Dtos;
using AuthService.Dtos;
using AuthService.Enums;

namespace AuthService.Repository;

public interface IAuthRepository
{
    Task RegisterUser(RegisterRequestDto request, CancellationToken cancellationToken);
    Task<UserProfileDto> Login(string email, string password, CancellationToken cancellationToken);
    Task Logout(string refreshToken, Guid tenantId, CancellationToken cancellationToken);
    Task<TokenResponseDto> RefreshTokenAsync(string refreshToken, Guid tenantId, CancellationToken cancellationToken);
    Task<IEnumerable<UserDto>> GetUsersByTenantId(Guid tenantId, CancellationToken cancellationToken);
    Task AddUserToTenant(Guid tenantId, Guid userId, TenantRole role, CancellationToken cancellationToken);
    Task RemoveUserFromTenant(Guid tenantId, Guid userId, CancellationToken cancellationToken);
}
