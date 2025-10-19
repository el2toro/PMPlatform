namespace Auth.API.Interfaces;

public interface IAuthRepository
{
    Task<User> RegisterUser(RegisterRequestDto request, CancellationToken cancellationToken);
    Task Login(RefreshToken refreshToken, CancellationToken cancellationToken);
    Task Logout(string refreshToken, Guid tenantId, CancellationToken cancellationToken);
    Task<TokenResponseDto> RefreshTokenAsync(string refreshToken, Guid tenantId, CancellationToken cancellationToken);
    Task<IEnumerable<User>> GetUsersByTenantId(Guid tenantId, CancellationToken cancellationToken);
    Task AddUserToTenant(Guid tenantId, Guid userId, TenantRole role, CancellationToken cancellationToken);
    Task RemoveUserFromTenant(Guid tenantId, Guid userId, CancellationToken cancellationToken);
}
