using Auth.API.Interfaces;

namespace Auth.API.Auth.RefreshToken;

public record RefreshTokenCommand(string RefreshToken, Guid TenantId) : IRequest<RefreshTokenResult>;
public record RefreshTokenResult(string RefreshToken, string AccessToken);
public class RefreshTokenHandler(IAuthRepository repository)
    : IRequestHandler<RefreshTokenCommand, RefreshTokenResult>
{
    public async Task<RefreshTokenResult> Handle(RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        var response = await repository.RefreshTokenAsync(command.RefreshToken, command.TenantId, cancellationToken);

        return new RefreshTokenResult(response.RefreshToken, response.AccessToken);
    }
}
