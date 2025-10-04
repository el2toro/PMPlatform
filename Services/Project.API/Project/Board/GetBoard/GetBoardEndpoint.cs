using Carter;
using MediatR;

namespace Project.API.Project.Board.GetBoard;

public class GetBoardEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("projects/{projectId:guid}/board", async (Guid projectId, ISender sender) =>
        {
            var result = await sender.Send(new GetBoardQuery(projectId));

            return Results.Ok(result.Board);
        });
    }
}
