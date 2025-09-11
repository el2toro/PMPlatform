namespace Auth.API.Auth.Users.GetUsersByTenantId;

public record GetUsersByTenantIdQuery(Guid TenantId) : IRequest<GetUsersByTenantIdResult>;
public record GetUsersByTenantIdResult(IEnumerable<UserDto> Users);
public class GetUsersByTenantIdHandler(IAuthRepository authRepository)
    : IRequestHandler<GetUsersByTenantIdQuery, GetUsersByTenantIdResult>
{
    public async Task<GetUsersByTenantIdResult> Handle(GetUsersByTenantIdQuery request, CancellationToken cancellationToken)
    {
        var users = await authRepository.GetUsersByTenantId(request.TenantId, cancellationToken);
        return new GetUsersByTenantIdResult(users);
    }
}
