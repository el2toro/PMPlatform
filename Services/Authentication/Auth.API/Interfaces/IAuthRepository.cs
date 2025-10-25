namespace Auth.API.Interfaces;

public interface IAuthRepository
{
    Task Login(RefreshToken refreshToken, CancellationToken cancellationToken);
    Task Logout(RefreshToken refreshToken, CancellationToken cancellationToken);
    Task<TokenResponseDto> RefreshTokenAsync(string refreshToken, Guid tenantId, CancellationToken cancellationToken);
    Task<RefreshToken> GetRefreshTokenAsync(Guid tenantId, string refreshToken, CancellationToken cancellationToken);
}
