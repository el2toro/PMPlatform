namespace AuthService.Dtos;

public record AuthResponse(
        Guid UserId,
        Guid TenantId,
        string Email,
        string Token,
        string RefreshToken,
        IEnumerable<string> Roles);

