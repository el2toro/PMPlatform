namespace Auth.API.Auth.RefreshToken;

public record RefreshTokenCommand(string RefreshToken, Guid TenantId) : IRequest<RefreshTokenResult>;
public record RefreshTokenResult(Guid UserId,
        Guid TenantId,
        string Email,
        string Token,
        string RefreshToken,
        IEnumerable<string> Roles);
public class RefreshTokenHandler(IAuthRepository repository)
    : IRequestHandler<RefreshTokenCommand, RefreshTokenResult>
{
    public async Task<RefreshTokenResult> Handle(RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        var response = await repository.RefreshTokenAsync(command.RefreshToken, command.TenantId, cancellationToken);
        return response.Adapt<RefreshTokenResult>();
    }
}
