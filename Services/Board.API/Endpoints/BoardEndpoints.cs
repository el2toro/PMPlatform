using Carter;
using MediatR;

namespace Board.API.Endpoints;

public class BoardEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("project/{projectId}/boards/{boardId}", async (Guid projectId, Guid boardId, ISender sender) =>
        {
            var board = await sender.Send("request");
            return Task.FromResult("it works");
        });

        app.MapGet("project/{projectId}/boards", async (Guid projectId, ISender sender) =>
        {
            var board = await sender.Send("request");
            return Task.FromResult("it works");
        });

        app.MapPost("project/{projectId}/boards", async (Guid projectId, ISender sender) =>
        {
            var board = await sender.Send("request");
            return Task.FromResult("it works");
        });

        app.MapPut("project/{projectId}/boards", async (Guid projectId, ISender sender) =>
        {
            var board = await sender.Send("request");
            return Task.FromResult("it works");
        });

        app.MapDelete("project/{projectId}/boards/{boardId}", async (Guid projectId, Guid boardId, ISender sender) =>
        {
            var board = await sender.Send("request");
            return Task.FromResult("it works");
        });
    }
}
