namespace Auth.API.Repository;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetUsersAsync(CancellationToken cancellationToken);
    Task<User> UpdateUsersAsync(User user, CancellationToken cancellationToken);
    Task<User> CreateUserAsync(User user, CancellationToken cancellationToken);
}
