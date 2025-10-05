using Tenant.API.Dtos;
using Tenant.API.Repository;

namespace Tenant.API.Tenant.CreateTenant;

public record CreateTenantCommand(TenantDto TenantDto) : IRequest<CreateTenantResult>;
public record CreateTenantResult(TenantDto TenantDto);
public class CreateTenantHandler(ITenantRepository tenantRepository)
    : IRequestHandler<CreateTenantCommand, CreateTenantResult>
{
    public async Task<CreateTenantResult> Handle(CreateTenantCommand command, CancellationToken cancellationToken)
    {
        var request = command.TenantDto.Adapt<Models.Tenant>();
        var createdTenant = await tenantRepository.CreateTenantAsync(request, cancellationToken);

        var result = createdTenant.Adapt<TenantDto>();
        return new CreateTenantResult(result);
    }
}
