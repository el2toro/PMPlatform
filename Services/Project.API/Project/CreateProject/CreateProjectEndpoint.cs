using Carter;
using Mapster;
using MediatR;

namespace Project.API.Project.CreateProject;

public record CreateProjectRequest(string Name, string Description);

public class CreateProjectEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/projects",
            async (CreateProjectRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateProjectCommand>();
            await sender.Send(command);
            //return Results.Created($"/projects/{project.Id}", project);
            return Results.Created();
        });
    }
}
