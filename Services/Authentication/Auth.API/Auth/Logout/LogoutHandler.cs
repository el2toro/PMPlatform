namespace Auth.API.Auth.Logout;

public record LogoutCommand(string RefreshToken, Guid TenantId) : ICommand<LogoutResult>;
public record LogoutResult(bool IsSuccess);

public class LogoutCommandValidator : AbstractValidator<LogoutCommand>
{
    public LogoutCommandValidator()
    {
        RuleFor(x => x.RefreshToken).NotEmpty().WithMessage("Refresh token is required");
        RuleFor(x => x.TenantId).NotEmpty().WithMessage("TenantId is required");
    }
}

public class LogoutHandler(IAuthRepository authRepository)
    : ICommandHandler<LogoutCommand, LogoutResult>
{
    public async Task<LogoutResult> Handle(LogoutCommand command, CancellationToken cancellationToken)
    {
        var existingRefreshToken = await authRepository
            .GetRefreshTokenAsync(command.TenantId, command.RefreshToken, cancellationToken)
            ?? throw new RefreshTokenNotFoundException("Refresh token was not found");

        existingRefreshToken.RevokedAt = DateTime.UtcNow;

        await authRepository.Logout(existingRefreshToken, cancellationToken);

        return new LogoutResult(true);
    }
}
