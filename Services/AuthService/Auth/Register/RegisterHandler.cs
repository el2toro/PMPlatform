namespace Auth.API.Auth.Register;

public record RegisterCommand(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    string TenantName) : IRequest<RegisterResult>;
public record RegisterResult(bool IsSuccess);

public class RegisterHandler(IAuthRepository repository)
    : IRequestHandler<RegisterCommand, RegisterResult>
{
    public async Task<RegisterResult> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        AuthService.Dtos.RegisterRequestDto request = command.Adapt<AuthService.Dtos.RegisterRequestDto>();
        await repository.RegisterUser(request, cancellationToken);

        return new RegisterResult(true);
    }
}
