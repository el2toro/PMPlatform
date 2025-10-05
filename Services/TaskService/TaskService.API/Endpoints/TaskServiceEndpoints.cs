using Microsoft.AspNetCore.Mvc;
using TaskService.Application.Tasks.Commands.UpdateTaskStatus;
using TaskService.Domain.Enums;

namespace TaskService.API.Endpoints;

public class TaskServiceEndpoints : ICarterModule
{
    public record UpdateTaskStatusRequest(Guid ProjectId, Guid TaskId, TaskItemStatus Status);
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("project/{projectId}/tasks/{taskId}", async (Guid projectId, Guid taskId, ISender sender) =>
        {
            var result = await sender.Send(new GetTaskByIdQuery(projectId, taskId));
            return Results.Ok(result.Task);
        });

        app.MapGet("project/{projectId}/tasks", async (Guid projectId, ISender sender) =>
        {
            var result = await sender.Send(new GetTasksQuery(projectId));
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

        app.MapPatch("project/{projectId}/tasks/{taskId}/status", async (UpdateTaskStatusRequest request, ISender sender) =>
        {
            var result = await sender.Send(new UpdateTaskStatusCommand(request.ProjectId, request.TaskId, request.Status));
            return Results.Ok(result.Task);
        });
    }
}
