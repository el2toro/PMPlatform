using Board.Application.Interfaces;
using Board.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;

namespace Board.Infrastructure.Repositories;

public class BoardRepository(BoardDbContext dbContext)
    : IBoardRepository
{
    public async Task<Domain.Entities.Board>
        CreateBoardAsync(Domain.Entities.Board board, CancellationToken cancellationToken)
    {
        //TODO: check if project exists
        var createdBoard = dbContext.Boards.Add(board).Entity;
        await dbContext.SaveChangesAsync(cancellationToken);

        return createdBoard;
    }

    public async Task DeleteBoardAsync(Guid projectId, Guid boardId, CancellationToken cancellationToken)
    {
        var board = await dbContext.Boards
            .FirstOrDefaultAsync(b => b.Id == boardId && b.ProjectId == projectId);

        ArgumentNullException.ThrowIfNull(board, nameof(board));

        dbContext.Boards.Remove(board);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Domain.Entities.Board>
        GetBoardByIdAsync(Guid projectId, Guid boardId, CancellationToken cancellationToken)
    {
        return await dbContext.Boards
            .Include(b => b.Columns)
            .FirstOrDefaultAsync(b => b.Id == boardId && b.ProjectId == projectId, cancellationToken)
            ?? new Domain.Entities.Board();
    }

    public async Task<IEnumerable<Domain.Entities.Board>> GetBoardsAsync(Guid projectId, CancellationToken cancellationToken)
    {
        return await dbContext.Boards.ToListAsync(cancellationToken);
    }

    public async Task<Domain.Entities.Board>
        UpdateBoardAsync(Domain.Entities.Board board, CancellationToken cancellationToken)
    {
        var existingBoard = await dbContext.Boards
             .FirstOrDefaultAsync(b => b.Id == board.Id && b.ProjectId == board.ProjectId);

        ArgumentNullException.ThrowIfNull(nameof(board.Id));

        existingBoard.Name = board.Name;
        existingBoard.Description = board.Description;
        existingBoard.UpdatedAt = DateTime.UtcNow;

        //TODO: chef if a new column is added or is updeted existing one
        existingBoard.Columns = board.Columns;

        dbContext.Boards.Update(existingBoard);
        await dbContext.SaveChangesAsync(cancellationToken);

        return existingBoard;
    }
}
