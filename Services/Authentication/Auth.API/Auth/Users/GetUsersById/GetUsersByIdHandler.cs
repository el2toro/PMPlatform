
namespace Auth.API.Auth.Users.GetUsersById;

public record GetUsersByIdQuery(Guid TenantId, IEnumerable<Guid> UserIds) : IRequest<GetUsersByIdResult>;
public record GetUsersByIdResult(IEnumerable<UserDto> Users);

//TODO: move user methods to user repository 
public class GetUsersByIdHandler(IAuthRepository authRepository)
    : IRequestHandler<GetUsersByIdQuery, GetUsersByIdResult>
{
    public async Task<GetUsersByIdResult> Handle(GetUsersByIdQuery request, CancellationToken cancellationToken)
    {
        var users = await authRepository.GetUsersById(request.TenantId, request.UserIds, cancellationToken);
        var usersDtos = MapToDto(users);
        return new GetUsersByIdResult(usersDtos);
    }

    //TODO: move to mapper, it is a duplicated method some is in GetUsersByTenantIdHandler
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
