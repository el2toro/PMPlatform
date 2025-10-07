namespace Project.API.Project.CreateProject;

public record CreateProjectRequest(Guid tenantId, string Name, string? Description, DateTime StartDate, DateTime EndDate);

public class CreateProjectEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("tenants/{tenantId:guid}/projects",
            async (Guid tenantId, CreateProjectRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateProjectCommand>();
            var response = await sender.Send(command);

            return Results.Created($"tenants/{response.ProjectDto.TenantId}/projects/{response.ProjectDto.Id}", response.ProjectDto);
        });
    }
}
