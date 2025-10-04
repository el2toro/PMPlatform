using Board.Application.Dtos;
using MediatR;

namespace Board.Application.BoardHandlers.Queries.GetBoards;

public record GetBoardsQuery(Guid ProjectId) : IRequest<GetBoardsResult>;
public record GetBoardsResult(IEnumerable<BoardDto> Boards);

public class GetBoardsHandler : IRequestHandler<GetBoardsQuery, GetBoardsResult>
{
    public Task<GetBoardsResult> Handle(GetBoardsQuery query, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
