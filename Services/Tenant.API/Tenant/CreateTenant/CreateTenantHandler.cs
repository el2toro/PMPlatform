using Tenant.API.Repository;

namespace Tenant.API.Tenant.CreateTenant;

public record CreateTenantCommand(string Name, string Description) : IRequest<CreateTenantResult>;
public record CreateTenantResult();
public class CreateTenantHandler(ITenantRepository tenantRepository)
    : IRequestHandler<CreateTenantCommand, CreateTenantResult>
{
    public async Task<CreateTenantResult> Handle(CreateTenantCommand command, CancellationToken cancellationToken)
    {
        var ownerId = Guid.Parse("E796730A-F617-4335-8E47-167B166AC67A"); // This should come from the authenticated user context

        await tenantRepository.CreateTenant(command.Name, command.Description, ownerId, cancellationToken);
        return new CreateTenantResult();
    }
}
