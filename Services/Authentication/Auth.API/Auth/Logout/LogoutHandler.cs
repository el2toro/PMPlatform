using Auth.API.Interfaces;

namespace Auth.API.Auth.Logout;

public record LogoutCommand(string RefreshToken, Guid TenantId) : IRequest<LogoutResult>;
public record LogoutResult(bool IsSuccess);

public class LogoutHandler(IAuthRepository repository)
    : IRequestHandler<LogoutCommand, LogoutResult>
{
    public async Task<LogoutResult> Handle(LogoutCommand command, CancellationToken cancellationToken)
    {
        await repository.Logout(command.RefreshToken, command.TenantId, cancellationToken);
        return new LogoutResult(true);
    }
}
