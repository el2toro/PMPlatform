using Microsoft.EntityFrameworkCore;
using Project.API.Data;
using Project.API.Dtos;
using Project.API.Dtos.Board;

namespace Project.API.Repository;

public class BoardRepository(ProjectDbContext dbContext) : IBoardRepository
{
    private readonly ProjectDbContext _dbContext = dbContext;

    public async Task<BoardDto> GetBoardAsync(Guid projectId, CancellationToken cancellationToken)
    {
        var board = await _dbContext.Boards
            .Include(c => c.Project)
            .Include(b => b.Columns)
            .ThenInclude(b => b.Tasks)
            .AsNoTracking()
            .Where(b => b.ProjectId == projectId)
            .Select(board => new BoardDto
            {
                Id = board.Id,
                Name = board.Name,
                Description = board.Description,
                ProjectId = board.ProjectId,
                ProjectName = board.Project.Name,
                ProjectDescription = board.Project.Description,
                Columns = board.Columns.Select(column => new ColumnDto
                {
                    Id = column.Id,
                    Name = column.Name,
                    BoardId = column.BoardId,
                    Tasks = column.Tasks.Select(task => new TaskItemDto
                    {
                        Id = task.Id,
                        Title = task.Title,
                        Description = task.Description,
                        TaskStatus = task.TaskStatus,
                        AssignedTo = task.AssignedTo,
                        ProjectId = task.ProjectId,
                        CreatedAt = task.CreatedAt,
                        CreatedBy = task.CreatedBy
                    })
                }),

            })
            .FirstOrDefaultAsync(cancellationToken);

        return board ?? new BoardDto();
    }
}
