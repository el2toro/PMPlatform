using Carter;
using Mapster;
using MediatR;
using Project.API.Enums;

namespace Project.API.Project.Task.UpdateTaskStatus;

public record UpdateTaskStatusRequest(Guid TaskId, Guid ColumnId, TaskItemStatus Status);
public class UpdateTaskStatusEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch("tasks/status", async (UpdateTaskStatusRequest request, ISender sender) =>
        {
            var command = request.Adapt<UpdateTaskStatusCommand>();
            var response = await sender.Send(command);
            return Results.Ok(response.TaskDto);
        });
    }
}
