
using Microsoft.EntityFrameworkCore;

namespace Auth.API.Repository;

public class UserRepository(AuthDbContext authDbContext) : IUserRepository
{
    public async Task<User> CreateUserAsync(User user, CancellationToken cancellationToken)
    {
        //TODO:  remove bcrypt and get password from the entity or generate a temporary password
        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword("Password123");
        var createdUser = authDbContext.Users.Add(user).Entity;

        authDbContext.UserTenants.Add(new UserTenant
        {
            //TODO: Get tenant id from logged in user
            UserId = createdUser.Id,
            TenantId = Guid.Parse("61DCEF47-B278-42A0-B983-08DDFD156343")
        });

        await authDbContext.SaveChangesAsync(cancellationToken);
        return createdUser;
    }

    public async Task<IEnumerable<User>> GetUsersAsync(CancellationToken cancellationToken)
    {
        return await authDbContext.Users.ToListAsync(cancellationToken);
    }

    public async Task<User> UpdateUsersAsync(User user, CancellationToken cancellationToken)
    {
        var existingUser = authDbContext.Users.FirstOrDefault(u => u.Id == user.Id);

        ArgumentNullException.ThrowIfNull(existingUser, nameof(existingUser));

        existingUser.FirstName = user.FirstName;
        existingUser.LastName = user.LastName;
        existingUser.Email = user.Email;
        existingUser.UpdatedAt = DateTime.UtcNow;

        authDbContext.Users.Update(existingUser);
        await authDbContext.SaveChangesAsync();

        return existingUser;
    }
}
