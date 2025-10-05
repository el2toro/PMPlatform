using Project.API.Dtos.Board;

namespace Project.API.Repository;

public interface IBoardRepository
{
    Task<BoardDto> GetBoardAsync(Guid projectId, CancellationToken cancellationToken);
}
