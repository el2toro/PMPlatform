namespace Project.API.Project.UpdateProject;

public class UpdateProjectEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("tenants/{tenantdId}/projects", async (Guid tenantId, ProjectDto request, ISender sender) =>
        {
            var response = await sender.Send(new UpdateProjectCommand(tenantId, request));

            return Results.Ok(response.ProjectDto);
        });
    }
}
