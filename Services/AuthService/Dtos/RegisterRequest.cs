namespace AuthService.Dtos;

public record RegisterRequest(string FirstName, string LastName, string Email, string Password, string TenantName);

