using AuthService.Data;
using AuthService.Dtos;
using AuthService.Enums;
using AuthService.Models;
using AuthService.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Repository;
public class AuthRepository : IAuthRepository
{
    private readonly AuthDbContext _authContext;
    private readonly IJwtTokenService _jwtTokenService;
    public AuthRepository(AuthDbContext authDbContext, IJwtTokenService jwtTokenService)
    {
        _authContext = authDbContext;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<UserProfileDto> Login(string email, string password, CancellationToken cancellationToken)
    {
        var user = await _authContext.Users
            .Include(u => u.UserTenants)
            .ThenInclude(ut => ut.Tenant)
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);

        ArgumentNullException.ThrowIfNull(user, "User not found");

        var isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);

        if (!isPasswordValid)
        {
            throw new Exception("Invalid password.");
        }

        var tenantId = user.UserTenants.FirstOrDefault()!.TenantId;
        var userRole = user.UserTenants.FirstOrDefault()!.Role;
        var token = _jwtTokenService.GenerateToken(user, userRole.ToString(), tenantId);
        var refreshToken = _jwtTokenService.GenerateRefreshToken(tenantId, user.Id);

        _authContext.RefreshTokens.Add(refreshToken);
        await _authContext.SaveChangesAsync(cancellationToken);

        return MapToUserProfile(user, token, refreshToken.Token, tenantId);
    }

    public async Task Logout(string refreshToken, Guid tenantId, CancellationToken cancellationToken)
    {
        var user = await _authContext.Users
        .Include(u => u.RefreshTokens)
        .SingleOrDefaultAsync(u => u.RefreshTokens.Any(t =>
            t.Token == refreshToken && t.TenantId == tenantId), cancellationToken);

        ArgumentNullException.ThrowIfNull(user, "User not found");

        var token = user.RefreshTokens.Single(t =>
            t.Token == refreshToken && t.TenantId == tenantId);

        token.RevokedAt = DateTime.UtcNow;

        await _authContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<UserProfileDto> RefreshTokenAsync(string refreshToken, Guid tenantId, CancellationToken cancellationToken)
    {
        using var transaction = await _authContext.Database.BeginTransactionAsync();

        var user = await _authContext.Users
        .Include(u => u.RefreshTokens)
        .Include(u => u.UserTenants)
        .ThenInclude(ut => ut.Tenant)
        .SingleOrDefaultAsync(u =>
            u.RefreshTokens.Any(t => t.Token == refreshToken && t.TenantId == tenantId));

        ArgumentNullException.ThrowIfNull(user, "User not found");

        RefreshToken oldToken = user.RefreshTokens
            .Single(t => t.Token == refreshToken && t.TenantId == tenantId && t.UserId == user.Id);

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

        return MapToUserProfile(user, jwtToken, newRefreshToken.Token, tenantId);
    }

    public async Task RegisterUser(RegisterRequestDto request, CancellationToken cancellationToken)
    {
        var user = _authContext.Users.FirstOrDefault(u => u.Email == request.Email);

        if (user != null)
        {
            throw new Exception("User with this email already exists.");
        }

        var newUser = CreateUser(request);

        var tenantId = await CreateTenant(request.TenantName);

        AssignUserToTenant(newUser.Id, tenantId);

        await _authContext.SaveChangesAsync(cancellationToken);
    }

    private User CreateUser(RegisterRequestDto request)
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

        _authContext.Users.Add(user);

        return user;
    }

    private async Task<Guid> CreateTenant(string tenantName)
    {
        var tenant = await _authContext.Tenants.FirstOrDefaultAsync(t => t.Name == tenantName);

        if (tenant is not null)
        {
            return tenant.TenantId;
        }

        tenant = new Tenant
        {
            Name = tenantName,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Domain = tenantName.ToLower() + ".com",
            Plan = TenantPlan.Free
        };

        _authContext.Tenants.Add(tenant);

        return tenant.TenantId;
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
            token,
            refreshToken,
            roles);
    }
}
