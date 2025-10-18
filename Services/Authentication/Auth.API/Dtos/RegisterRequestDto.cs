namespace Auth.API.Dtos;

public record RegisterRequestDto(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string TenantName);

