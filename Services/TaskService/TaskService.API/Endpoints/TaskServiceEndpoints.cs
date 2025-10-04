using Microsoft.AspNetCore.Mvc;

namespace TaskService.API.Endpoints;

public class TaskServiceEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("project/{projectId}/tasks/{taskId}", async (Guid projectId, Guid taskId, ISender sender) =>
        {
            var result = await sender.Send(new GetTaskByIdQuery(projectId, taskId));
            return Results.Ok(result.Task);
        });

        app.MapGet("project/{projectId}/tasks", async (Guid projectId, ISender sender) =>
        {
            var result = await sender.Send(new GetTasksByProjectIdQuery(projectId));
            return Results.Ok(result.Tasks);
        });

        app.MapPost("project/{projectId}/tasks", async ([FromBody] TaskItemDto request, ISender sender) =>
        {
            var result = await sender.Send(new CreateTaskCommand(request));
            return Results.Ok(result.Task);
        });

        app.MapPut("project/{projectId}/tasks", async ([FromBody] TaskItemDto request, ISender sender) =>
        {
            var result = await sender.Send(new UpdateTaskCommand(request));
            return Results.Ok(result.Task);
        });

        app.MapDelete("project/{projectId}/tasks/{taskId}", async (Guid projectId, Guid taskId, ISender sender) =>
        {
            var result = await sender.Send(new DeleteTaskCommand(projectId, taskId));
            return Results.Ok(result);
        });
    }
}
