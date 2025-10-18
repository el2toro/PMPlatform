using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Users.GetUsersById;

public record GetUsersByIdRequest(Guid TenantId, IEnumerable<Guid> UserIds);

public class GetUsersByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/users/tenant/{tenantId}", async (Guid tenantId, [FromQuery] Guid[] userIds, ISender sender) =>
        {
            var users = await sender.Send(new GetUsersByIdQuery(tenantId, userIds));
            return Results.Ok(users.Users);
        });
    }
}
