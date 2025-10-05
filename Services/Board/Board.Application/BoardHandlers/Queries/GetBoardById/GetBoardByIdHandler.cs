namespace Board.Application.BoardHandlers.Queries.GetBoardById;

public record GetBoardByIdQuery(Guid ProjectId, Guid BoardId) : IRequest<GetBoardByIdResult>;
public record GetBoardByIdResult(BoardDto Board);
public class GetBoardByIdHandler(IBoardRepository boardRepository)
    : IRequestHandler<GetBoardByIdQuery, GetBoardByIdResult>
{
    public async Task<GetBoardByIdResult> Handle(GetBoardByIdQuery query, CancellationToken cancellationToken)
    {
        var board = await boardRepository.GetBoardByIdAsync(query.ProjectId, query.BoardId, cancellationToken);

        var boardDto = new BoardDto
        {
            Id = board.Id,
            Name = board.Name,
            Description = board.Description,
            ProjectId = board.ProjectId,
            //ProjectName = board.Project.Name,
            // ProjectDescription = board.Project.Description,
            Columns = board.Columns.Select(column => new ColumnDto
            {
                Id = column.Id,
                Name = column.Name,
                BoardId = column.BoardId
            }),
        };

        //  var result = board.Adapt<BoardDto>();

        return new GetBoardByIdResult(boardDto);
    }
}
