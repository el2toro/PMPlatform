using Mapster;
using MediatR;
using Project.API.Dtos;
using Project.API.Repository;

namespace Project.API.Project.Task.GetTasksByColumId;

public record GetTasksByColumnIdQuery(Guid ColumnId) : IRequest<GetTasksByColumnIdResult>;

public record GetTasksByColumnIdResult(IEnumerable<TaskItemDto> Tasks);

public class GetTasksByColumnIdHandler(ITaskRepository taskRepository)
    : IRequestHandler<GetTasksByColumnIdQuery, GetTasksByColumnIdResult>
{
    public async Task<GetTasksByColumnIdResult> Handle(GetTasksByColumnIdQuery query, CancellationToken cancellationToken)
    {
        var tasks = await taskRepository.GeTasksByColumnIdAsync(query.ColumnId, cancellationToken);
        var result = tasks.Adapt<IEnumerable<TaskItemDto>>();
        return new GetTasksByColumnIdResult(result);
    }
}
