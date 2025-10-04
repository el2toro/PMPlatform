
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
        return new GetUsersByIdResult(users);
    }
}
