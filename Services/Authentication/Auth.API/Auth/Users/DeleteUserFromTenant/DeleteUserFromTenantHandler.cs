namespace Auth.API.Auth.Users.DeleteUserFromTenant;

public record DeleteUserFromTenantCommand(Guid UserId, Guid TenantId) : IRequest<DeleteUserFromTenantResult>;
public record DeleteUserFromTenantResult(bool IsSuccess);
public class DeleteUserFromTenantHandler(IAuthRepository authRepository)
    : IRequestHandler<DeleteUserFromTenantCommand, DeleteUserFromTenantResult>
{
    public async Task<DeleteUserFromTenantResult> Handle(DeleteUserFromTenantCommand command, CancellationToken cancellationToken)
    {
        await authRepository.RemoveUserFromTenant(command.TenantId, command.UserId, cancellationToken);
        return new DeleteUserFromTenantResult(true);
    }
}

