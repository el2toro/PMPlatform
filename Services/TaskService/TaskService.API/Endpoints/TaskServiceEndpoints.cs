using Microsoft.AspNetCore.Mvc;
using TaskService.Application.Tasks.Commands.UpdateTaskStatus;
using TaskService.Application.Tasks.Queries.GetAllTasks;
using TaskService.Application.Tasks.Queries.GetTaskCount;
using TaskService.Application.Tasks.Queries.GetUserAssignedTasks;
using TaskService.Domain.Enums;

namespace TaskService.API.Endpoints;

public class TaskServiceEndpoints : ICarterModule
{
    public record UpdateTaskStatusRequest(Guid ProjectId, Guid TaskId, TaskItemStatus Status);

    //TODO: Add endpoint details: name, description, produce response...
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("tenants/{tenantId}/projects/{projectId}/tasks/{taskId}",
            async (Guid tenantId, Guid projectId, Guid taskId, ISender sender) =>
        {
            var result = await sender.Send(new GetTaskByIdQuery(projectId, taskId));
            return Results.Ok(result.Task);
        });

        app.MapGet("tenants/{tenantId}/projects/{projectId}/tasks",
            async (Guid tenantId, Guid projectId, ISender sender) =>
        {
            var result = await sender.Send(new GetTasksQuery(projectId));
            return Results.Ok(result.Tasks);
        });

        app.MapPost("tenants/{tenantId}/projects/{projectId}/tasks",
            async (Guid tenantId, Guid projectId, [FromBody] TaskItemDto request, ISender sender) =>
        {
            //TODO: check if route projectId == request.ProjectId
            var result = await sender.Send(new CreateTaskCommand(request));
            return Results.Ok(result.Task);
        });

        app.MapPut("tenants/{tenantId}/projects/{projectId}/tasks",
            async (Guid tenantId, Guid projectId, [FromBody] TaskItemDto request, ISender sender) =>
        {
            var result = await sender.Send(new UpdateTaskCommand(request));
            return Results.Ok(result.Task);
        });

        app.MapDelete("tenants/{tenantId}/projects/{projectId}/tasks/{taskId}",
            async (Guid tenantId, Guid projectId, Guid taskId, ISender sender) =>
        {
            var result = await sender.Send(new DeleteTaskCommand(projectId, taskId));
            return Results.Ok(result);
        });

        app.MapPatch("tenants/{tenantId}/projects/{projectId}/tasks/{taskId}/status",
            async (Guid tenantId, UpdateTaskStatusRequest request, ISender sender) =>
        {
            var result = await sender.Send(new UpdateTaskStatusCommand(request.ProjectId, request.TaskId, request.Status));
            return Results.Ok(result.Task);
        });

        app.MapGet("tenants/{tenantId}/projects/{projectId}/tasks/progress",
            async (Guid tenantId, Guid projectId, ISender sender) =>
        {
            var result = await sender.Send(new GetTaskCountQuery(projectId));
            return Results.Ok(result.TaskCount);
        });

        app.MapGet("tenants/{tenantId}/tasks",
        async (Guid tenantId, ISender sender) =>
        {
            var result = await sender.Send(new GetAllTasksQuery(tenantId));
            return Results.Ok(result.Tasks);
        });

        app.MapGet("tenants/{tenantId}/tasks/users/{userId}",
        async (Guid tenantId, Guid userId, ISender sender) =>
        {
            var result = await sender.Send(new GetUserAssignedTasksQuery(tenantId, userId));
            return Results.Ok(result.Tasks);
        });
    }
}
