using FluentValidation;

namespace Board.Application.BoardHandlers.Commands.CreateBoard;

public record CreateBoardCommand(BoardDto Board) : IRequest<CreateBoardResult>;
public record CreateBoardResult(BoardDto Board);

public class CreateBoardCommandValidator : AbstractValidator<CreateBoardCommand>
{
    public CreateBoardCommandValidator()
    {

    }
}

public class CreateBoardHandler(IBoardRepository boardRepository)
    : IRequestHandler<CreateBoardCommand, CreateBoardResult>
{
    public async Task<CreateBoardResult> Handle(CreateBoardCommand command, CancellationToken cancellationToken)
    {
        var board = command.Board.Adapt<Board.Domain.Entities.Board>();

        var createdBoard = await boardRepository.CreateBoardAsync(board, cancellationToken);
        var result = createdBoard.Adapt<BoardDto>();

        return new CreateBoardResult(result);
    }
}
