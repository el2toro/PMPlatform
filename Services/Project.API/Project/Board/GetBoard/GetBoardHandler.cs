using Mapster;
using MediatR;
using Project.API.Dtos.Board;
using Project.API.Repository;

namespace Project.API.Project.Board.GetBoard;

public record GetBoardQuery(Guid ProjectId) : IRequest<GetBoardResult>;

public record GetBoardResult(BoardDto Board);

public class GetBoardHandler(IBoardRepository boardRepository)
    : IRequestHandler<GetBoardQuery, GetBoardResult>
{
    public async Task<GetBoardResult> Handle(GetBoardQuery query, CancellationToken cancellationToken)
    {
        var board = await boardRepository.GetBoardAsync(query.ProjectId, cancellationToken);

        return new GetBoardResult(board);
    }
}
