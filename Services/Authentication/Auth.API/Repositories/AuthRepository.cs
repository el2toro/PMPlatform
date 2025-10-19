using Auth.API.Interfaces;
using Auth.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Auth.API.Repository;

public class AuthRepository(AuthDbContext authDbContext,
    IJwtTokenService jwtTokenService,
    TenantServiceClient tenantServiceClient) : IAuthRepository
{
    private readonly AuthDbContext _authContext = authDbContext;
    private readonly IJwtTokenService _jwtTokenService = jwtTokenService;
    private readonly TenantServiceClient _tenantServiceClient = tenantServiceClient;

    public async Task Login(RefreshToken refreshToken, CancellationToken cancellationToken)
    {
        _authContext.RefreshTokens.Add(refreshToken);
        await _authContext.SaveChangesAsync(cancellationToken);
    }

    public async Task Logout(string refreshToken, Guid tenantId, CancellationToken cancellationToken)
    {
        var user = await _authContext.Users
        .Include(u => u.RefreshTokens)
        .SingleOrDefaultAsync(u => u.RefreshTokens.Any(t =>
            t.Token == refreshToken), cancellationToken); //TODO: add tenantId condition && t.TenantId == tenantId

        ArgumentNullException.ThrowIfNull(user, "User not found");

        var token = user.RefreshTokens.Single(t =>
            t.Token == refreshToken); //TODO: add tenantId condition && t.TenantId == tenantId

        token.RevokedAt = DateTime.UtcNow;

        await _authContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<TokenResponseDto> RefreshTokenAsync(string refreshToken, Guid tenantId, CancellationToken cancellationToken)
    {
        using var transaction = await _authContext.Database.BeginTransactionAsync();

        var user = await _authContext.Users
        .Include(u => u.RefreshTokens)
        .Include(u => u.UserTenants)
        //.ThenInclude(ut => ut.Tenant)
        .SingleOrDefaultAsync(u =>
            u.RefreshTokens.Any(t => t.Token == refreshToken)); //TODO: add tenantId condition && t.TenantId == tenantId

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

    public async Task<User> RegisterUser(RegisterRequestDto request, CancellationToken cancellationToken)
    {
        var user = await _authContext.Users.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

        if (user != null)
        {
            throw new Exception("User with this email already exists.");
        }

        var createdUser = CreateUser(request, cancellationToken);
        await _authContext.SaveChangesAsync(cancellationToken);

        return createdUser;
    }

    public async Task<IEnumerable<User>> GetUsersByTenantId(Guid tenantId, CancellationToken cancellationToken)
    {
        return await _authContext.Users
            .AsNoTracking()
            .Where(u => u.UserTenants.Any(ut => ut.TenantId == tenantId))
            .ToListAsync();
    }

    public async Task AddUserToTenant(Guid tenantId, Guid userId, TenantRole role, CancellationToken cancellationToken)
    {
        _authContext.UserTenants.Add(new UserTenant
        {
            TenantId = tenantId,
            UserId = userId,
            Role = role
        });

        await _authContext.SaveChangesAsync(cancellationToken);
    }

    public async Task RemoveUserFromTenant(Guid tenantId, Guid userId, CancellationToken cancellationToken)
    {
        await _authContext.UserTenants
            .Where(ut => ut.TenantId == tenantId && ut.UserId == userId)
            .ExecuteDeleteAsync(cancellationToken);
    }

    private User CreateUser(RegisterRequestDto request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        return _authContext.Users.Add(user).Entity;
    }

    private void AssignUserToTenant(Guid userId, Guid tenantId)
    {
        _authContext.UserTenants.Add(new UserTenant
        {
            UserId = userId,
            TenantId = tenantId,
            Role = TenantRole.Admin,
            CreatedAt = DateTime.UtcNow
        });
    }

    private UserProfileDto MapToUserProfile(User user, string token, string refreshToken, Guid tenantId)
    {
        var roles = user.UserTenants.Select(ut => ut.Role.ToString());

        return new UserProfileDto(
            user.Id,
            tenantId,
            user.Email,
            user.FirstName,
            user.LastName,
            token,
            refreshToken,
            roles);
    }
}
