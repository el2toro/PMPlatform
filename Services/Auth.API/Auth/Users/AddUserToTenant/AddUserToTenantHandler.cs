using AuthService.Enums;

namespace Auth.API.Auth.Users.AddUserToTenant;

public record AddUserToTenantCommand(Guid TenantId, Guid UserId, TenantRole Role) : IRequest<AddUserToTenantResult>;
public record AddUserToTenantResult(bool IsSuccess);
public class AddUserToTenantHandler(IAuthRepository authRepository)
    : IRequestHandler<AddUserToTenantCommand, AddUserToTenantResult>
{
    public async Task<AddUserToTenantResult> Handle(AddUserToTenantCommand command, CancellationToken cancellationToken)
    {
        await authRepository.AddUserToTenant(command.TenantId, command.UserId, command.Role, cancellationToken);
        return new AddUserToTenantResult(true);
    }
}
