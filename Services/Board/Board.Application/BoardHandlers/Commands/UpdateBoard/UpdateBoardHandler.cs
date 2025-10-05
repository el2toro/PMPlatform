namespace Board.Application.BoardHandlers.Commands.UpdateBoard;

public record UpdateBoardCommand(BoardDto Board) : IRequest<UpdateBoardResult>;
public record UpdateBoardResult(BoardDto Board);
public class CreateBoardHandler(IBoardRepository boardRepository)
    : IRequestHandler<UpdateBoardCommand, UpdateBoardResult>
{
    public async Task<UpdateBoardResult> Handle(UpdateBoardCommand command, CancellationToken cancellationToken)
    {
        var board = command.Board.Adapt<Board.Domain.Entities.Board>();

        var updatedBoard = await boardRepository.UpdateBoardAsync(board, cancellationToken);
        var result = updatedBoard.Adapt<BoardDto>();

        return new UpdateBoardResult(result);
    }
}
