namespace Auth.API.Dtos;

public record RegisterResponseDto
{
    public Guid UserId { get; init; }
    public string FirstName { get; init; } = default!;
    public string LastName { get; init; } = default!;
    public string Email { get; init; } = default!;
    public string TenantName { get; init; } = default!;
}
