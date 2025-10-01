
using Microsoft.EntityFrameworkCore;

namespace Auth.API.Repository;

public class UserRepository(AuthDbContext authDbContext) : IUserRepository
{
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
