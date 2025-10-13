namespace Project.API.Project.GetProjectById;

public class GetProjectByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("tenants/{tenantId}/projects/{projectId}",
            async ([FromRoute] Guid tenantId, [FromRoute] Guid projectId, [FromServices] ISender sender) =>
        {
            var result = await sender.Send(new GetProjectByIdQuery(projectId, tenantId));
            return Results.Ok(result.Project);
        });
    }
}
