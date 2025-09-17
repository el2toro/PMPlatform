using Carter;
using MediatR;

namespace Project.API.Project.GetProjectById;

public class GetProjectByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/project/{projectId}/{tenantId}", async (Guid projectId, Guid tenantId, ISender sender) =>
        {
            var result = await sender.Send(new GetProjectByIdQuery(projectId, tenantId));
            return Results.Ok(result.Project);
        });
    }
}
