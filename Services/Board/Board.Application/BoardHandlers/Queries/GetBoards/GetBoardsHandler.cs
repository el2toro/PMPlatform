namespace Board.Application.BoardHandlers.Queries.GetBoards;

public record GetBoardsQuery(Guid ProjectId) : IRequest<GetBoardsResult>;
public record GetBoardsResult(IEnumerable<BoardDto> Boards);

public class GetBoardsHandler(IBoardRepository boardRepository)
    : IRequestHandler<GetBoardsQuery, GetBoardsResult>
{
    public async Task<GetBoardsResult> Handle(GetBoardsQuery query, CancellationToken cancellationToken)
    {
        var boards = await boardRepository.GetBoardsAsync(query.ProjectId, cancellationToken);
        var result = boards.Adapt<IEnumerable<BoardDto>>();

        return new GetBoardsResult(result);
    }
}
