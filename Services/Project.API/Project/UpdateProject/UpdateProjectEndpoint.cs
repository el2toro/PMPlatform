using Carter;
using MediatR;

namespace Project.API.Project.UpdateProject;

public class UpdateProjectEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("api/projects", (ISender sender) =>
        {

        });
    }
}
