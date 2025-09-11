using Tenant.API.Dtos;
using Tenant.API.Services;

namespace Tenant.API.Tenant.GetTenantUsers;

public record GetTenantUsersQuery(Guid TenantId) : IRequest<GetTenantUsersResult>;
public record GetTenantUsersResult(IEnumerable<UserDto> Users);

public class GetTenantUsersHandler(AuthServiceClient authServiceClient)
    : IRequestHandler<GetTenantUsersQuery, GetTenantUsersResult>
{
    public async Task<GetTenantUsersResult> Handle(GetTenantUsersQuery query, CancellationToken cancellationToken)
    {
        var users = await authServiceClient.GetUsersByTenantIdAsync(query.TenantId);
        return new GetTenantUsersResult(users);
    }
}
