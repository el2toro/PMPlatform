
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Users.GetUsersAssignedToProject;

public class GetUsersAssignedToProjectEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("tenants/{tenantId}/projects/{projectId}/users",
            async (Guid tenantId, Guid projectId, [FromQuery] Guid[] userIds, ISender sender) =>
        {
            var result = await sender.Send(new GetUsersAssignedToProjectQuery(tenantId, userIds));
            return Results.Ok(result.Users);
        });
    }
}
