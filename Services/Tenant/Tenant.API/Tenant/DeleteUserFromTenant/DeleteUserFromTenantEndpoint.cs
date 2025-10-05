namespace Tenant.API.Tenant.DeleteUserFromTenant;

public class DeleteUserFromTenantEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/tenants/{tenantId}/users/{userId}",
            async (Guid userId, Guid tenantId, ISender sender) =>
            {
                await sender.Send(new RemoveUserFromTenantCommand(userId, tenantId));
                return Results.NoContent();
            });
    }
}
