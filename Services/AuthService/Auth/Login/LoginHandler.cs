namespace Auth.API.Auth.Login;
public record LoginCommand(string Email, string Password) : IRequest<LoginResult>;
public record LoginResult(
        Guid UserId,
        Guid TenantId,
        string Email,
        string FirstName,
        string LastName,
        string Token,
        string RefreshToken,
        IEnumerable<string> Roles);

public class LoginHandler(IAuthRepository repository)
    : IRequestHandler<LoginCommand, LoginResult>
{
    public async Task<LoginResult> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var response = await repository.Login(command.Email, command.Password, cancellationToken);
        return response.Adapt<LoginResult>();
    }
}
