using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Project.API.Dtos;

namespace Project.API.Project.Task.CreateTask;

public record CreateTaskRequest(Guid ProjectId, string Title, string? Description, DateTime DueDate);

public class CreateTaskEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/tasks", async ([FromBody] TaskItemDto request, ISender sender) =>
        {
            // var command = request.Adapt<CreateTaskCommand>();
            var response = await sender.Send(new CreateTaskCommand(request));
            return Results.Ok(response.Task);
        });
    }
}
