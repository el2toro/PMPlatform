using Board.Application.BoardHandlers.Commands.CreateBoard;
using Board.Application.BoardHandlers.Commands.DeleteBoard;
using Board.Application.BoardHandlers.Commands.UpdateBoard;
using Board.Application.BoardHandlers.Queries.GetBoardById;
using Board.Application.BoardHandlers.Queries.GetBoards;
using Board.Application.Dtos;
using Carter;
using MediatR;

namespace Board.API.Endpoints;

public class BoardEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("projects/{projectId}/boards/{boardId}", async (Guid projectId, Guid boardId, ISender sender) =>
        {
            var result = await sender.Send(new GetBoardByIdQuery(projectId, boardId));
            return Results.Ok(result.Board);
        });

        app.MapGet("projects/{projectId}/boards", async (Guid projectId, ISender sender) =>
        {
            var result = await sender.Send(new GetBoardsQuery(projectId));
            return Results.Ok(result.Boards);
        });

        app.MapPost("project/{projectId}/boards", async (BoardDto request, ISender sender) =>
        {
            var result = await sender.Send(new CreateBoardCommand(request));
            return Results.Created($"project/{result.Board.ProjectId}/boards/{result.Board.Id}", result.Board);
        });

        app.MapPut("project/{projectId}/boards", async (BoardDto request, ISender sender) =>
        {
            var result = await sender.Send(new UpdateBoardCommand(request));
            return Results.Ok(result.Board);
        });

        app.MapDelete("project/{projectId}/boards/{boardId}", async (Guid projectId, Guid boardId, ISender sender) =>
        {
            var result = await sender.Send(new DeleteBoardCommand(projectId, boardId));
            return Results.NoContent();
        });
    }
}
