using Tenant.API.Enum;
using Tenant.API.Services;

namespace Tenant.API.Tenant.AddUserToTenant;

public record AddUserToTenantCommand(Guid TenantId, Guid UserId, TenantRole TenantRole) : IRequest<AddUserToTenantResult>;
public record AddUserToTenantResult(bool IsSuccess);

public class AddUserToTenantHandler(AuthServiceClient authServiceClient)
    : IRequestHandler<AddUserToTenantCommand, AddUserToTenantResult>
{
    public async Task<AddUserToTenantResult> Handle(AddUserToTenantCommand command, CancellationToken cancellationToken)
    {
        try
        {
            await authServiceClient.AddUserToTenant(command.TenantId, command.UserId, command.TenantRole);
            return new AddUserToTenantResult(true);
        }
        catch (Exception)
        {
            throw;
        }
    }
}
