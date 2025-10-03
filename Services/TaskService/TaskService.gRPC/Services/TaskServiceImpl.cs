namespace TaskService.gRPC.Services;

public class TaskServiceImpl(ISender sender)
    : TaskService.TaskServiceBase
{
    private readonly ISender _sender = sender;

    public override async Task<TaskResponse> CreateTask(CreateTaskRequest request, ServerCallContext context)
    {
        var command = MapRequestToDto(request);
        var result = await _sender.Send(new CreateTaskCommand(command));

        return MapDtoToResponse(result.Task);
    }

    public override async Task<TaskResponse> UpdateTask(UpdateTaskRequest request, ServerCallContext context)
    {
        var command = MapRequestToDto(request);
        var result = await _sender.Send(new UpdateTaskCommand(command));

        return MapDtoToResponse(result.Task);
    }

    public override async Task<GetTasksResponse> GetTasks(GetTasksRequest request, ServerCallContext context)
    {
        var result = await _sender.Send(new GetTasksQuery(Guid.Parse(request.ProjectId)));

        var response = MapGetTasksResultToResponse(result);

        return response;
    }

    public override async Task<TaskResponse> GetTaskById(GetTaskByIdRequest request, ServerCallContext context)
    {
        var result = await _sender.Send(new GetTaskByIdQuery(Guid.Parse(request.TaskId)));

        return MapDtoToResponse(result.Task);
    }

    public override async Task<DeleteTaskResponse> DeleteTask(DeleteTaskRequest request, ServerCallContext context)
    {
        var result = await _sender.Send(new DeleteTaskCommand(Guid.Parse(request.TaskId)));
        var response = result.Adapt<DeleteTaskResponse>();

        return response;
    }

    //TODO: map with mapster

    private TaskResponse MapDtoToResponse(TaskItemDto task)
    {
        var response = task.Adapt<TaskResponse>();

        foreach (var subtask in task.Subtasks)
        {
            var subtaskItem = new Subtask
            {
                Id = subtask.Id.ToString(),
                IsComplited = subtask.IsCompleted,
                Title = subtask.Title,
                TaskId = subtask.TaskId.ToString()
            };

            response.Subtasks.Add(subtaskItem);
        }

        foreach (var comment in task.Comments)
        {
            var commentItem = new Comment
            {
                Id = comment.Id.ToString(),
                Content = comment.Content,
                CommentedBy = comment.CommentedBy.ToString(),
                CreatedAt = comment.CreatedAt.ToString(),
                UpdatedAt = comment.UpdatedAt.ToString(),
                TaskId = comment.TaskId.ToString()
            };

            response.Comments.Add(commentItem);
        }

        foreach (var attachement in task.Attachments)
        {
            var attachementItem = new Attachement
            {
                Id = attachement.Id.ToString(),
                ContentType = attachement.ContentType,
                FileData = ByteString.CopyFrom(attachement.FileData),
                FileName = attachement.FileName,
                TaskId = attachement.TaskId.ToString()
            };

            response.Attachements.Add(attachementItem);
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
            Comments = request.Comments.Select(c => new CommentDto { Content = c.Content }),
            Attachments = request.Attachements.Select(a => new AttachmentDto
            {
                FileName = a.FileName,
                ContentType = a.ContentType,
                FileData = a.FileData.ToByteArray()
            })
        };
    }

    private GetTasksResponse MapGetTasksResultToResponse(GetTasksResult result)
    {
        var response = new GetTasksResponse();
        foreach (var task in result.Tasks)
        {
            var taskResponse = MapDtoToResponse(task);

            response.Tasks.Add(taskResponse);
        }

        return response;
    }

    private TaskItemDto MapRequestToDto(UpdateTaskRequest request)
    {
        return new TaskItemDto
        {
            Id = Guid.Parse(request.Id),
            Title = request.Title,
            Description = request.Description,
            DueDate = DateTime.Parse(request.DueDate),
            AssignedTo = Guid.Parse(request.AssignedTo),
            TaskStatus = (TaskItemStatus)request.TaskStatus,
            Subtasks = request.Subtasks.Select(st =>
            new SubtaskDto
            {
                Id = Guid.Parse(st.Id),
                Title = st.Title,
                IsCompleted = st.IsComplited,
                TaskId = Guid.Parse(st.TaskId)
            }),
            Comments = request.Comments.Select(c =>
            new CommentDto
            {
                Id = Guid.Parse(c.Id),
                Content = c.Content,
                TaskId = Guid.Parse(c.TaskId),
                CommentedBy = Guid.Parse(c.CommentedBy),
                CreatedAt = DateTime.Parse(c.CreatedAt),
                UpdatedAt = DateTime.Parse(c.UpdatedAt),
            }),
            Attachments = request.Attachements.Select(a =>
            new AttachmentDto
            {
                FileName = a.FileName,
                ContentType = a.ContentType,
                FileData = a.FileData.ToByteArray(),
                Id = Guid.Parse(a.Id),
                TaskId = Guid.Parse(a.TaskId)
            })
        };
    }
}
