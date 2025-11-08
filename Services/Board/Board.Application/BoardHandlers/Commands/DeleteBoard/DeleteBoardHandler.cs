using FluentValidation;

namespace Board.Application.BoardHandlers.Commands.DeleteBoard;

public record DeleteBoardCommand(Guid ProjectId, Guid BoardId) : IRequest<DeleteBoardResult>;
public record DeleteBoardResult(bool IsSuccess);

public class DeleteBoardCommandValidator : AbstractValidator<DeleteBoardCommand>
{
    public DeleteBoardCommandValidator()
    {

    }
}

public class DeleteBoardHandler(IBoardRepository boardRepository)
    : IRequestHandler<DeleteBoardCommand, DeleteBoardResult>
{
    public async Task<DeleteBoardResult> Handle(DeleteBoardCommand command, CancellationToken cancellationToken)
    {
        await boardRepository.DeleteBoardAsync(command.ProjectId, command.BoardId, cancellationToken);
        return new DeleteBoardResult(true);
    }
}
