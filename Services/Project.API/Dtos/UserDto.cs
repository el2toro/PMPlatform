namespace Project.API.Dtos;

public record UserDto
{
    public Guid Id { get; init; }
    public string UserName { get; init; } = default!;
    public string Image { get; init; } = "portrait1.jpg";
}
