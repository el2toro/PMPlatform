
using Auth.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Auth.API.Repository;

public class UserRepository(AuthDbContext authDbContext) : IUserRepository
{
    public async Task<User> CreateUserAsync(Guid tenantId, User user, CancellationToken cancellationToken)
    {
        var createdUser = authDbContext.Users.Add(user).Entity;

        authDbContext.UserTenants.Add(new UserTenant
        {
            UserId = createdUser.Id,
            TenantId = tenantId,
            CreatedAt = DateTime.UtcNow,
            Role = TenantRole.Contributor
        });

        await authDbContext.SaveChangesAsync(cancellationToken);
        return createdUser;
    }

    public async Task<IEnumerable<User>> GetUsersAsync(Guid tenantId, CancellationToken cancellationToken)
    {
        return await authDbContext.Users
            .AsNoTracking()
            .Where(u => u.UserTenants.Any(ut => ut.TenantId == tenantId))
            .ToListAsync(cancellationToken);
    }

    public async Task<User> UpdateUsersAsync(Guid tenantId, User user, CancellationToken cancellationToken)
    {
        var existingUser = authDbContext.Users.Update(user).Entity;
        await authDbContext.SaveChangesAsync(cancellationToken);

        return existingUser;
    }

    public async Task<User> GetUserByEmail(string email, CancellationToken cancellationToken)
    {
        return await authDbContext.Users
            .Include(u => u.UserTenants)
            .AsNoTracking()
            .Where(u => u.Email == email)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<User>> GetUsersById(Guid tenantId, IEnumerable<Guid> userIds, CancellationToken cancellationToken)
    {
        return await authDbContext.Users
            .AsNoTracking()
            .Where(u => u.UserTenants.Any(ut => ut.TenantId == tenantId) && userIds.Contains(u.Id))
            .ToListAsync(cancellationToken);
    }

    public async Task<User> GetUserById(Guid tenantId, Guid userId, CancellationToken cancellationToken)
    {
        return await authDbContext.Users
            .AsNoTracking()
            .Where(u => u.UserTenants.Any(ut => ut.TenantId == tenantId) && u.Id == userId)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
