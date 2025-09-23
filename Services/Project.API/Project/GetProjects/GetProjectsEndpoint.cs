using Carter;
using MediatR;

namespace Project.API.Project.GetProjects;

public class GetProjectsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/projects", async (ISender sender) =>
        {
            var result = await sender.Send(new GetProjectsQuery());
            return Results.Ok(result.Projects);
        })
        .WithTags("Projects")
        .WithName("GetProjects")
        //.Produces<List<ProjectResponse>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden)
        .RequireAuthorization();
    }
}
