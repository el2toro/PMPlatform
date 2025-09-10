namespace AuthService.Dtos;

public class UserProfileResponse(
        Guid UserId,
        string Email,
        string Name,
        Guid TenantId,
        IEnumerable<string> Roles);

