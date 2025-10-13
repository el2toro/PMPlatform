namespace Project.API.Project.UpdateProject;

public class UpdateProjectEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("tenants/{tenantId}/projects", async ([FromRoute] Guid tenantId,
            [FromBody] ProjectDto request,
            [FromServices] ISender sender) =>
        {
            var response = await sender.Send(new UpdateProjectCommand(tenantId, request));

            return Results.Ok(response.ProjectDto);
        });
    }
}
