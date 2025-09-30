using Microsoft.EntityFrameworkCore;
using Project.API.Data;
using Project.API.Models;

namespace Project.API.Repository;

public class BoardRepository(ProjectDbContext dbContext) : IBoardRepository
{
    private readonly ProjectDbContext _dbContext = dbContext;

    public async Task<Board> GetBoardAsync(Guid projectId)
    {
        var board = await _dbContext.Boards
            .Include(b => b.Columns)
            // .ThenInclude(b => b.Tasks)
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.ProjectId == projectId);

        return board ?? new Board();
    }
}
