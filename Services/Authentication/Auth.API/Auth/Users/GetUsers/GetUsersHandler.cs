
using Auth.API.Repository;

namespace Auth.API.Auth.Users.GetUsers;

public record GetUsersQuery() : IRequest<GetUsersResult>;
public record GetUsersResult(IEnumerable<UserDto> Users);

public class GetUsersHandler(IUserRepository userRepository)
    : IRequestHandler<GetUsersQuery, GetUsersResult>
{
    public async Task<GetUsersResult> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await userRepository.GetUsersAsync(cancellationToken);
        var result = users.Adapt<IEnumerable<UserDto>>();
        return new GetUsersResult(result);
    }
}
