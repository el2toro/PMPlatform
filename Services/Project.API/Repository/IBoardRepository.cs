using Project.API.Models;

namespace Project.API.Repository;

public interface IBoardRepository
{
    Task<Board> GetBoardAsync(Guid projectId);
}
