namespace Tenant.API.Tenant.GetTenantUsers;

public class GetTenantUsersEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/tenants/{tenantId:guid}/users", async (Guid tenantId, ISender sender) =>
        {
            var users = await sender.Send(new GetTenantUsersQuery(tenantId));

            return Results.Ok(users.Users);
        });
    }
}
