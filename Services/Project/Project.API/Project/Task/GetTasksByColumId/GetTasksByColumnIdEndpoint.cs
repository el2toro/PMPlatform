using Carter;
using MediatR;

namespace Project.API.Project.Task.GetTasksByColumId;

public class GetTasksByColumnIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        //TODO: add project id, task id?
        app.MapGet("api/projects/tasks/board/columns/{columnId:guid}", async (Guid columnId, ISender sender) =>
        {
            var result = await sender.Send(new GetTasksByColumnIdQuery(columnId));
            return Results.Ok(result.Tasks);
        });
    }
}
