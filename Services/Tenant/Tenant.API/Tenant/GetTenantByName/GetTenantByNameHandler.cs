using Tenant.API.Dtos;
using Tenant.API.Repository;

namespace Tenant.API.Tenant.GetTenantByName;

public record GetTenantByNameQuery(string TenantName) : IRequest<GetTenantByNameResult>;
public record GetTenantByNameResult(TenantDto? Tenant);

public class GetTenantByNameHandler(ITenantRepository tenantRepository)
    : IRequestHandler<GetTenantByNameQuery, GetTenantByNameResult>
{
    public async Task<GetTenantByNameResult> Handle(GetTenantByNameQuery query, CancellationToken cancellationToken)
    {
        var tenant = await tenantRepository.GetTenantByNameAsync(query.TenantName, cancellationToken);
        return new GetTenantByNameResult(tenant?.Adapt<TenantDto>());
    }
}
