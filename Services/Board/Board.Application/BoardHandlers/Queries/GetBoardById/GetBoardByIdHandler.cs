using Board.Application.Dtos;
using Board.Application.Interfaces;
using MediatR;

namespace Board.Application.BoardHandlers.Queries.GetBoardById;

public record GetBoardByIdQuery(Guid ProjectId, Guid BoardId) : IRequest<GetBoardByIdResult>;
public record GetBoardByIdResult(BoardDto Board);
public class GetBoardByIdHandler(IBoardRepository boardRepository)
    : IRequestHandler<GetBoardByIdQuery, GetBoardByIdResult>
{
    public async Task<GetBoardByIdResult> Handle(GetBoardByIdQuery query, CancellationToken cancellationToken)
    {
        //var board = await boardRepository.GetBoardById(query.ProjectId, query.BoardId, cancellationToken);
        return new GetBoardByIdResult(new BoardDto());
    }
}
