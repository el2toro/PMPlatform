using Carter;
using MediatR;
using Project.API.Dtos;

namespace Project.API.Project.UpdateProject;

public class UpdateProjectEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("api/projects", async (ProjectDto request, ISender sender) =>
        {
            ///var command = request.Adapt<UpdateProjectCommand>();
            var response = await sender.Send(new UpdateProjectCommand(request));

            return Results.Ok(response.ProjectDto);
        });
    }
}
