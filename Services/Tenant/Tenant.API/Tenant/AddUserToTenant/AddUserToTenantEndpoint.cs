using Tenant.API.Enum;

namespace Tenant.API.Tenant.AddUserToTenant;

public record AddUserToTenantRequest(Guid TenantId, Guid UserId, TenantRole TenantRole);

public class AddUserToTenantEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/tenants/{tenantId}/users/{userId}",
            async (AddUserToTenantRequest request, ISender sender) =>
        {
            var command = request.Adapt<AddUserToTenantCommand>();
            await sender.Send(command);
            return Results.NoContent();
        });
    }
}
