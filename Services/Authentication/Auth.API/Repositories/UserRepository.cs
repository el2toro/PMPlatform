
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
        var existingUser = authDbContext.Users
            .FirstOrDefault(u => u.Id == user.Id && user.UserTenants.Any(ut => ut.TenantId == tenantId));

        existingUser.FirstName = user.FirstName;
        existingUser.LastName = user.LastName;
        existingUser.Email = user.Email;
        existingUser.UpdatedAt = DateTime.UtcNow;

        // authDbContext.Users.Update(existingUser);
        await authDbContext.SaveChangesAsync();

        return existingUser;
    }

    public async Task<User> GetUserByEmail(string email, CancellationToken cancellationToken)
    {
        return await authDbContext.Users
            .AsNoTracking()
            .Where(u => u.Email == email)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
