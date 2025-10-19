namespace Auth.API.Users.GetUsersById;

public record GetUsersByIdRequest(Guid TenantId, IEnumerable<Guid> UserIds);

public class GetUserByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("tenants/{tenantId}/users/{userId}", async (Guid tenantId, Guid userId, ISender sender) =>
        {
            var users = await sender.Send(new GetUserByIdQuery(tenantId, userId));
            return Results.Ok(users.User);
        });
    }
}
