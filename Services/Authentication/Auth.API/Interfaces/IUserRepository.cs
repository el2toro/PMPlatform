namespace Auth.API.Interfaces;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetUsersAsync(Guid tenantId, CancellationToken cancellationToken);
    Task<User> UpdateUsersAsync(Guid tenantId, User user, CancellationToken cancellationToken);
    Task<User> CreateUserAsync(Guid tenantId, User user, CancellationToken cancellationToken);
    Task<User> GetUserByEmail(string email, CancellationToken cancellationToken);
    Task<IEnumerable<User>> GetUsersById(Guid tenantId, IEnumerable<Guid> userIds, CancellationToken cancellationToken);
    Task<User> GetUserById(Guid tenantId, Guid userId, CancellationToken cancellationToken);
    Task AddUserToTenant(Guid tenantId, Guid userId, TenantRole role, CancellationToken cancellationToken);
    Task<IEnumerable<User>> GetUsersByTenantId(Guid tenantId, CancellationToken cancellationToken);
    Task DeleteUserFromTenant(Guid tenantId, Guid userId, CancellationToken cancellationToken);
    Task DeleteUser(Guid tenantId, Guid userId, CancellationToken cancellationToken);
    Task<User> RegisterUser(User user, CancellationToken cancellationToken);
}
