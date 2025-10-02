using Board.gRPC.DataContext;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

namespace Board.gRPC.Services;

public class BoardServiceImplementation(ILogger<BoardServiceImplementation> logger, BoardDbContext dbContext)
    : BoardService.BoardServiceBase
{
    private readonly ILogger<BoardServiceImplementation> _logger = logger;
    private readonly BoardDbContext _dbContext = dbContext;

    public override async Task<BoardResponse> GetBoard(BoadRequest request, ServerCallContext context)
    {
        //TODO: GetBordById, And GetBoards
        var board = await _dbContext.Boards
           .Include(b => b.Columns)
           .AsNoTracking()
           .FirstOrDefaultAsync(b => b.ProjectId == Guid.Parse(request.ProjectId));

        var result = new BoardResponse
        {
            Id = board?.Id.ToString(),
            Name = board?.Name,
            Description = board?.Description,
            CreatedBy = board?.CreatedBy.ToString()
        };

        result.Columns.AddRange(board?.Columns.Select(column => new BoardColumn
        {
            Id = column.Id.ToString(),
            Name = column.Name
        }));

        return result;
    }
}
