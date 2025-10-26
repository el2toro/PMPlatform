namespace Auth.API.Auth.RefreshToken;

public record RefreshTokenCommand(string RefreshToken, Guid TenantId) : ICommand<RefreshTokenResult>;
public record RefreshTokenResult(string RefreshToken, string AccessToken);

public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
{
    public RefreshTokenCommandValidator()
    {
        RuleFor(x => x.RefreshToken).NotEmpty().WithMessage("Refresh token is required");
        RuleFor(x => x.TenantId).NotEmpty().WithMessage("TenantId is required");
    }
}
public class RefreshTokenHandler(IAuthRepository repository)
    : ICommandHandler<RefreshTokenCommand, RefreshTokenResult>
{
    public async Task<RefreshTokenResult> Handle(RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        var response = await repository.RefreshTokenAsync(command.RefreshToken, command.TenantId, cancellationToken);

        return new RefreshTokenResult(response.RefreshToken, response.AccessToken);
    }
}
