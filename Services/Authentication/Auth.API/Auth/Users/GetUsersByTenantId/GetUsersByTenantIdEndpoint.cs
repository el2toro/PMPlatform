namespace Auth.API.Auth.Users.GetUsersByTenantId;

public class GetUsersByTenantIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/users/tenant/{tenantId:guid}", async (Guid tenantId, ISender sender) =>
        {
            var users = await sender.Send(new GetUsersByTenantIdQuery(tenantId));

            return Results.Ok(users.Users);
        });
    }
}
