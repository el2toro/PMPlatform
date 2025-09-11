namespace Auth.API.Auth.Users.DeleteUserFromTenant;

public class DeleteUserFromTenantEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/users/{userId}/tenants/{tenantId}",
            async (Guid userId, Guid tenantId, ISender sender) =>
            {
                await sender.Send(new DeleteUserFromTenantCommand(userId, tenantId));
                return Results.NoContent();
            });
    }
}

