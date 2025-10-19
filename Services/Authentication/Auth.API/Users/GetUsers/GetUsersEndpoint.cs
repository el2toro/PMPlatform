namespace Auth.API.Users.GetUsers;

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
    }
}
