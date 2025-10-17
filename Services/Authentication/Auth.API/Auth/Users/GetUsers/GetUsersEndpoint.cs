namespace Auth.API.Auth.Users.GetUsers;

public class GetUsersEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("tenants/{tenantId}/users", async (Guid tenantId, ISender sender) =>
        {
            var result = await sender.Send(new GetUsersQuery(tenantId));
            return Results.Ok(result.Users);
        })
        .WithDisplayName("GetUsesByTenantId");

        app.MapGet("tenants/{tenantId}/Projects/{projectId}/users",
            async (Guid tenantId, Guid projectId, ISender sender) =>
        {
            var result = await sender.Send(new GetUsersQuery(tenantId));
            return Results.Ok(result.Users);
        });
    }
}
