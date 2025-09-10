namespace AuthService.Dtos;

public record UserProfileDto(
        Guid UserId,
        Guid TenantId,
        string Email,
        string Token,
        string RefreshToken,
        IEnumerable<string> Roles);

