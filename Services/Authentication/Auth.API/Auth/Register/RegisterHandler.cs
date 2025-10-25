namespace Auth.API.Auth.Register;

public record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string TenantName) : ICommand<RegisterResult>;
public record RegisterResult(RegisterResponseDto RegisterDto);

public class RegisterHandler(IUserRepository userRepository,
    TenantServiceClient tenantServiceClient)
    : ICommandHandler<RegisterCommand, RegisterResult>
{
    public async Task<RegisterResult> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        User user = await userRepository.GetUserByEmail(command.Email, cancellationToken);

        //Cannot create a user if a user with the same email already exists
        if (user is not null)
            throw new InvalidOperationException($"User with this email: {nameof(command.Email)} already exists.");

        user = MapCommandToUser(command);
        User registeredUser = await userRepository.RegisterUser(user, cancellationToken);

        //Get potential tenant where user will be assigned
        Tenant? tenant = await tenantServiceClient.GetTenantByName(command.TenantName);
        Tenant tenantRequest = MapDtoToTenant(command.TenantName, registeredUser!.Id);

        // Create tenant if it does not exists
        tenant ??= await tenantServiceClient.CreateTenant(tenantRequest);

        // Add user to tenant as owner, since they created the tenant
        await userRepository.AddUserToTenant(tenant.TenantId, registeredUser.Id, TenantRole.Owner, cancellationToken);

        RegisterResponseDto result = MapUserToRegisterDto(registeredUser, tenant.Name);

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
            FirstName = user?.FirstName!,
            LastName = user?.LastName!,
            Email = user?.Email!,
            TenantName = tenantName
        };
    }

    private User MapCommandToUser(RegisterCommand command)
    {
        return new User
        {
            FirstName = command.FirstName,
            LastName = command.LastName,
            Email = command.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(command.Password),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
    }
}
