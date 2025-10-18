using Auth.API.Interfaces;

namespace Auth.API.Auth.Register;

public record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string TenantName) : IRequest<RegisterResult>;
public record RegisterResult(RegisterResponseDto RegisterDto);

public class RegisterHandler(IAuthRepository repository, TenantServiceClient tenantServiceClient)
    : IRequestHandler<RegisterCommand, RegisterResult>
{
    public async Task<RegisterResult> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        RegisterRequestDto registerDto = command.Adapt<RegisterRequestDto>();
        User createdUser = await repository.RegisterUser(registerDto, cancellationToken);

        Tenant? tenant = await tenantServiceClient.GetTenantByName(registerDto.TenantName);

        Tenant tenantRequest = MapDtoToTenant(registerDto.TenantName, createdUser.Id);

        // Create tenant if it does not exist
        tenant ??= await tenantServiceClient.CreateTenant(tenantRequest);

        // Add user to tenant as admin, since they created the tenant, to be reviewed
        await repository.AddUserToTenant(tenant.TenantId, createdUser.Id, TenantRole.Admin, cancellationToken);

        RegisterResponseDto result = MapUserToRegisterDto(createdUser, tenant.Name);

        return new RegisterResult(result);
    }

    private Tenant MapDtoToTenant(string tenantName, Guid ownerId) => new()
    {
        Name = tenantName,
        OwnerId = ownerId,
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    };

    private RegisterResponseDto MapUserToRegisterDto(User user, string tenantName)
    {
        return new RegisterResponseDto
        {
            UserId = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            TenantName = tenantName
        };
    }
}
