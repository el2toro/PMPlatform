using Auth.API.Exceptions;
using Auth.API.Interfaces;
using Core.CQRS;
using FluentValidation;
using System.Security.Authentication;

namespace Auth.API.Auth.Login;

public record LoginCommand(string Email, string Password) : ICommand<LoginResult>;
public record LoginResult(
        Guid UserId,
        Guid TenantId,
        string Email,
        string FirstName,
        string LastName,
        string Token,
        string RefreshToken,
        IEnumerable<string> Roles);

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required");
        RuleFor(x => x.Email).EmailAddress().WithMessage("Not valid email address");
        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
    }
}

internal class LoginHandler(IAuthRepository repository,
    IUserRepository userRepository,
    IJwtTokenService jwtTokenService)
    : ICommandHandler<LoginCommand, LoginResult>
{
    public async Task<LoginResult> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserByEmail(command.Email, cancellationToken)
            ?? throw new UserNotFoundException(command.Email);

        if (!IsValidPassword(command.Password, user.PasswordHash))
            throw new InvalidCredentialException();

        Guid tenantId = user.UserTenants.FirstOrDefault()!.TenantId;
        TenantRole userRole = user.UserTenants.FirstOrDefault()!.Role;
        string token = jwtTokenService.GenerateToken(user, userRole.ToString(), tenantId);
        var refreshToken = jwtTokenService.GenerateRefreshToken(tenantId, user.Id);

        await repository.Login(refreshToken, cancellationToken);

        var result = MapToDto(user, tenantId, token, refreshToken, userRole);

        return result;
    }

    private bool IsValidPassword(string enteredPassword, string storedHash)
    {
        return BCrypt.Net.BCrypt.Verify(enteredPassword, storedHash);
    }

    private LoginResult MapToDto(User user, Guid tenantId, string token, Models.RefreshToken refreshToken, TenantRole role)
    {
        return new LoginResult(
            UserId: user.Id,
            TenantId: tenantId,
            Email: user.Email,
            FirstName: user.FirstName,
            LastName: user.LastName,
            Token: token,
            RefreshToken: refreshToken.Token,
            Roles: new List<string> { role.ToString() }
        );
    }
}
