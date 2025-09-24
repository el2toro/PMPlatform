using Carter;
using Mapster;
using MediatR;
using Project.API.Dtos;

namespace Project.API.Project.Task.UpdateTask;

//public record UpdateTaskRequest(Guid TaskId, string Title, string Description, DateTime DueDate);

public class UpdateTaskEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("api/tasks", async (TaskItemDto request, ISender sender) =>
        {
            //var command = request.Adapt<UpdateTaskCommand>();
            var response = await sender.Send(new UpdateTaskCommand(request));
            return Results.Ok(response.TaskDto);
        });
    }
}
