using AuthService.Data;
using AuthService.Dtos;
using AuthService.Enums;
using AuthService.Models;
using AuthService.Services;
using Microsoft.EntityFrameworkCore;

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

    public async Task<AuthResponse> Login(AuthRequest request)
    {
        var user = await _authContext.Users
            .Include(u => u.UserTenants)
            .ThenInclude(ut => ut.Tenant)
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        ArgumentNullException.ThrowIfNull(user, "User not found");
        var isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);

        if (!isPasswordValid)
        {
            throw new Exception("Invalid password.");
        }

        var tenantId = user.UserTenants.FirstOrDefault()!.TenantId;
        var userRole = user.UserTenants.FirstOrDefault()!.Role;
        var token = _jwtTokenService.GenerateToken(user, userRole.ToString(), tenantId);
        var refreshToken = _jwtTokenService.GenerateRefreshToken(tenantId);

        _authContext.RefreshTokens.Add(refreshToken);

        //TODO: Add proper mapping
        return new AuthResponse
        (
             user.Id,
             tenantId,
             user.Email,
             token,
             refreshToken.Token,
             [userRole.ToString()]
        );
    }

    public Task<AuthResponse> Logout(Guid userId)
    {
        throw new NotImplementedException();
    }

    public async Task<AuthResponse> RefreshTokenAsync(string refreshToken, Guid tenantId)
    {
        var user = await _authContext.Users
        .Include(u => u.RefreshTokens)
        .SingleOrDefaultAsync(u =>
            u.RefreshTokens.Any(t => t.Token == refreshToken && t.TenantId == tenantId));

        ArgumentNullException.ThrowIfNull(user, "User not found");

        var oldToken = user.RefreshTokens.Single(t => t.Token == refreshToken && t.TenantId == tenantId);

        ArgumentNullException.ThrowIfNull(oldToken, "Invalid refresh token");

        oldToken.RevokedAt = DateTime.UtcNow;

        var newRefreshToken = _jwtTokenService.GenerateRefreshToken(tenantId);
        user.RefreshTokens.Add(newRefreshToken);

        await _authContext.SaveChangesAsync();

        return new AuthResponse(
            user.Id,
                tenantId,
                user.Email,
                _jwtTokenService.GenerateToken(user, user.UserTenants.FirstOrDefault()!.Role.ToString(), tenantId),
                newRefreshToken.Token,
                [user.UserTenants.FirstOrDefault()!.Role.ToString()]
            );

    }

    public async Task RegisterUser(RegisterRequest request)
    {
        var user = _authContext.Users.FirstOrDefault(u => u.Email == request.Email);

        if (user != null)
        {
            throw new Exception("User with this email already exists.");
        }

        //TODO: Validate tenant exists
        //var tenant = _authContext.Tenants.FirstOrDefault(t => t.Id == request.TenantId);

        //Move BCrypto to a dedicated service
        var newUser = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
        };

        _authContext.Users.Add(newUser);

        //TODO: check if user already has a tenant if not assign it to a default one
        var tenant = new Tenant
        {
            Name = request.TenantName,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            Domain = request.TenantName.ToLower() + ".com",
            Plan = TenantPlan.Free
        };

        _authContext.Tenants.Add(tenant);

        _authContext.UserTenants.Add(new UserTenant
        {
            UserId = newUser.Id,
            TenantId = tenant.TenantId,
            Role = TenantRole.Admin,
            CreatedAt = DateTime.UtcNow
        });

        //TODO: Assign the user to the tenant

        await _authContext.SaveChangesAsync();
    }

    public async Task RevokeTokenAsync(string refreshToken, Guid tenantId)
    {
        var user = await _authContext.Users
        .Include(u => u.RefreshTokens)
        .SingleOrDefaultAsync(u => u.RefreshTokens.Any(t =>
            t.Token == refreshToken && t.TenantId == tenantId));

        ArgumentNullException.ThrowIfNull(user, "User not found");

        var token = user.RefreshTokens.Single(t =>
            t.Token == refreshToken && t.TenantId == tenantId);

        token.RevokedAt = DateTime.UtcNow;

        await _authContext.SaveChangesAsync();
    }
}
