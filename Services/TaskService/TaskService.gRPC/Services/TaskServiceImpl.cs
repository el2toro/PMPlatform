using Azure;
using Grpc.Core;
using Mapster;
using MediatR;
using TaskService.Application.Dtos;
using TaskService.Application.Tasks.Commands.CreateTask;
using TaskService.Domain.Enums;

namespace TaskService.gRPC.Services;

public class TaskServiceImpl(ISender sender)
    : TaskService.TaskServiceBase
{
    private readonly ISender _sender = sender;

    public override async Task<CreateTaskResponse> CreateTask(CreateTaskRequest request, ServerCallContext context)
    {
        var command = MapRequestToDto(request);
        var result = await _sender.Send(new CreateTaskCommand(command));

        //TODO: Add attachments

        var response = MapCreateTaskResultToResponse(result);

        return response;
    }

    //TODO: map with mapster
    private CreateTaskResponse MapCreateTaskResultToResponse(CreateTaskResult result)
    {
        var response = result.TaskItem.Adapt<CreateTaskResponse>();

        foreach (var subtask in result.TaskItem.Subtasks)
        {
            var subtaskItem = new Subtask
            {
                Id = subtask.Id.ToString(),
                IsComplited = subtask.IsCompleted,
                Title = subtask.Title
            };

            response.Subtasks.Add(subtaskItem);
        }

        foreach (var comment in result.TaskItem.Comments)
        {
            var commentItem = new Comment
            {
                Id = comment.Id.ToString(),
                Content = comment.Content,
                CommentedBy = comment.CommentedBy.ToString(),
                CreatedAt = comment.CreatedAt.ToString(),
                UpdatedAt = comment.UpdatedAt.ToString(),
            };

            response.Comments.Add(commentItem);
        }

        return response;
    }

    private TaskItemDto MapRequestToDto(CreateTaskRequest request)
    {
        return new TaskItemDto
        {
            Title = request.Title,
            Description = request.Description,
            ProjectId = Guid.Parse(request.ProjectId),
            DueDate = DateTime.Parse(request.DueDate),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            CreatedBy = Guid.Parse(request.CreatedBy),
            UpdatedBy = Guid.Parse(request.UpdatedBy),
            AssignedTo = Guid.Parse(request.AssignedTo),
            TaskStatus = (TaskItemStatus)request.TaskStatus,
            Subtasks = request.Subtasks.Select(st => new SubtaskDto { Title = st.Title }),
            Comments = request.Comments.Select(c => new CommentDto { Content = c.Content })
        };
    }
}
