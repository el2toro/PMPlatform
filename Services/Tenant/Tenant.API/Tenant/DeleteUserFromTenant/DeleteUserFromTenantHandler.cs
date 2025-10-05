using Tenant.API.Services;

namespace Tenant.API.Tenant.DeleteUserFromTenant;

public record RemoveUserFromTenantCommand(Guid UserId, Guid TenantId) : IRequest<RemoveUserFromTenantResult>;
public record RemoveUserFromTenantResult(bool IsSuccess);
public class DeleteUserFromTenantHandler(AuthServiceClient authServiceClient)
    : IRequestHandler<RemoveUserFromTenantCommand, RemoveUserFromTenantResult>
{
    public async Task<RemoveUserFromTenantResult> Handle(RemoveUserFromTenantCommand command, CancellationToken cancellationToken)
    {
        await authServiceClient.RemoveUserFromTenant(command.TenantId, command.UserId);
        return new RemoveUserFromTenantResult(true);
    }
}
