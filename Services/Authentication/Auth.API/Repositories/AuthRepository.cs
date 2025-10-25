using Microsoft.IdentityModel.Tokens;

namespace Auth.API.Repositories;

public class AuthRepository(AuthDbContext authDbContext,
    IJwtTokenService jwtTokenService)
    : IAuthRepository
{
    private readonly AuthDbContext _authContext = authDbContext;
    private readonly IJwtTokenService _jwtTokenService = jwtTokenService;

    public async Task Login(RefreshToken refreshToken, CancellationToken cancellationToken)
    {
        _authContext.RefreshTokens.Add(refreshToken);
        await _authContext.SaveChangesAsync(cancellationToken);
    }

    public async Task Logout(RefreshToken refreshToken, CancellationToken cancellationToken)
    {
        _authContext.RefreshTokens.Update(refreshToken);
        await _authContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<TokenResponseDto> RefreshTokenAsync(string refreshToken, Guid tenantId, CancellationToken cancellationToken)
    {
        using var transaction = await _authContext.Database.BeginTransactionAsync();

        var user = await _authContext.Users
        .Include(u => u.RefreshTokens)
        .Include(u => u.UserTenants)
        .SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == refreshToken), cancellationToken); //TODO: add tenantId condition && t.TenantId == tenantId

        ArgumentNullException.ThrowIfNull(user, "User not found");

        RefreshToken oldToken = user.RefreshTokens
            .Single(t => t.Token == refreshToken); //TODO: add tenantId condition && t.TenantId == tenantId

        ArgumentNullException.ThrowIfNull(oldToken);

        if (!oldToken.IsActive)
            throw new SecurityTokenException("Refresh token already revoked");

        oldToken.RevokedAt = DateTime.UtcNow;

        RefreshToken newRefreshToken = _jwtTokenService.GenerateRefreshToken(tenantId, user.Id);
        _authContext.RefreshTokens.Update(oldToken);
        await _authContext.RefreshTokens.AddAsync(newRefreshToken);
        await _authContext.SaveChangesAsync(cancellationToken);

        await transaction.CommitAsync(cancellationToken);

        string jwtToken = _jwtTokenService.GenerateToken(user, user.UserTenants.FirstOrDefault()!.Role.ToString(), tenantId);
        List<string> roles = [user.UserTenants.FirstOrDefault()!.Role.ToString()];

        return new TokenResponseDto(newRefreshToken.Token, jwtToken);
    }

    public async Task<RefreshToken> GetRefreshTokenAsync(Guid tenantId, string refreshToken, CancellationToken cancellationToken)
    {
        return await authDbContext.Users
        .Include(u => u.RefreshTokens)
        .Where(u => u.UserTenants.Any(t => t.TenantId == tenantId) && u.RefreshTokens.Any(t => t.Token == refreshToken))
        .Select(u => u.RefreshTokens.SingleOrDefault())
        .SingleOrDefaultAsync(cancellationToken) ?? default!;
    }
}
