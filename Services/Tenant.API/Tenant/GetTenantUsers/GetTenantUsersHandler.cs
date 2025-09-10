using Tenant.API.Dtos;
using Tenant.API.Repository;

namespace Tenant.API.Tenant.GetTenantUsers;

public record GetTenantUsersQuery(Guid TenantId) : IRequest<GetTenantUsersResult>;
public record GetTenantUsersResult(IEnumerable<UserDto> Users);

public class GetTenantUsersHandler(ITenantRepository repository)
    : IRequestHandler<GetTenantUsersQuery, GetTenantUsersResult>
{
    public async Task<GetTenantUsersResult> Handle(GetTenantUsersQuery query, CancellationToken cancellationToken)
    {
        var users = await repository.GetTenantUsers(query.TenantId);
        return new GetTenantUsersResult(users);
    }
}
