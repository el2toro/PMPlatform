using Auth.API.Interfaces;

namespace Auth.API.Users.GetUsers;

public record GetUsersQuery(Guid TenantId) : IRequest<GetUsersResult>;
public record GetUsersResult(IEnumerable<UserDto> Users);

public class GetUsersHandler(IUserRepository userRepository)
    : IRequestHandler<GetUsersQuery, GetUsersResult>
{
    public async Task<GetUsersResult> Handle(GetUsersQuery query, CancellationToken cancellationToken)
    {
        var users = await userRepository.GetUsersAsync(query.TenantId, cancellationToken);
        var result = MapToDto(users);
        return new GetUsersResult(result);
    }

    //TODO: move to mapper, it is a duplicated method some is in GetUsersByIdHandler
    private IEnumerable<UserDto> MapToDto(IEnumerable<User> users)
    {
        return users.Select(user => new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            FullName = $"{user.FirstName} {user.LastName}"
        });
    }
}
