namespace Project.API.Dtos;

public record PaginatedResponse
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);
    public IEnumerable<ProjectDto> Items { get; set; } = [];
}
