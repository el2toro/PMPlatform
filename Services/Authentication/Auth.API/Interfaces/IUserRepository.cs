namespace Auth.API.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetUsersAsync(Guid tenantId, CancellationToken cancellationToken);
    Task<User> UpdateUsersAsync(Guid tenantId, User user, CancellationToken cancellationToken);
    Task<User> CreateUserAsync(Guid tenantId, User user, CancellationToken cancellationToken);
    Task<User> GetUserByEmail(string email, CancellationToken cancellationToken);
}
