using Tenant.API.Repository;

namespace Tenant.API.Tenant.GetTenantById;

public record GetTenantByIdQuery(Guid TenantId) : IRequest<GetTenantByIdResult?>;
public record GetTenantByIdResult(Guid TenantId, string Name, string Description, Guid OwnerId, DateTime CreatedAt, DateTime OpdatedAt);
public class GetTenantByIdHandler(ITenantRepository repository)
    : IRequestHandler<GetTenantByIdQuery, GetTenantByIdResult?>
{
    public async Task<GetTenantByIdResult?> Handle(GetTenantByIdQuery query, CancellationToken cancellationToken)
    {
        var tenant = await repository.GetTenantByIdAsync(query.TenantId, cancellationToken);

        return new GetTenantByIdResult(
            tenant.Id,
            tenant.Name,
            tenant.Description!,
            tenant.OwnerId,
            tenant.CreatedAt,
            tenant.UpdatedAt);
    }
}
