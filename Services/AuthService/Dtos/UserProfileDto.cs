namespace AuthService.Dtos;

public record UserProfileDto(
        Guid UserId,
        Guid TenantId,
        string Email,
        string FirstName,
        string LastName,
        string Token,
        string RefreshToken,
        IEnumerable<string> Roles);

