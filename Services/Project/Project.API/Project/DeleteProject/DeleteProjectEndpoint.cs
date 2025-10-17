namespace Project.API.Project.DeleteProject;

public record DeleteProjectRequest(Guid TenantId, Guid ProjectId);

public class DeleteProjectEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("tenants/{tenantId}/projects/{projectId}",
            async ([FromRoute] Guid tenantId,
            [FromRoute] Guid projectId,
            [FromBody] DeleteProjectRequest request,
            [FromServices] ISender sender) =>
        {
            var result = await sender.Send(new DeleteProjectCommand(request.TenantId, request.ProjectId));
            return Results.NoContent();
        });
    }
}
