using Carter;
using MediatR;
using Project.API.Dtos;

namespace Project.API.Project.GetProjects;

public class GetProjectsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/projects", async ([AsParameters] PaginationRequest request, ISender sender) =>
        {
            var result = await sender.Send(new GetProjectsQuery(request.PageNumber, request.PageSize));
            return Results.Ok(result.PaginatedResponse);
        })
        .WithTags("Projects")
        .WithName("GetProjects")
        //.Produces<List<ProjectResponse>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden);
        // .RequireAuthorization();
    }
}
