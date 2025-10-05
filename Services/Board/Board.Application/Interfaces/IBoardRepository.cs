namespace Board.Application.Interfaces;

public interface IBoardRepository
{
    Task<IEnumerable<Domain.Entities.Board>> GetBoardsAsync(Guid projectId, CancellationToken cancellationToken);
    Task<Domain.Entities.Board> GetBoardByIdAsync(Guid projectId, Guid boardId, CancellationToken cancellationToken);
    Task<Domain.Entities.Board> CreateBoardAsync(Domain.Entities.Board board, CancellationToken cancellationToken);
    Task<Domain.Entities.Board> UpdateBoardAsync(Domain.Entities.Board board, CancellationToken cancellationToken);
    Task DeleteBoardAsync(Guid projectId, Guid boardId, CancellationToken cancellationToken);
}
