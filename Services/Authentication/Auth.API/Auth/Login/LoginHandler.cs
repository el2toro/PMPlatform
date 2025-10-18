using Auth.API.Exceptions;
using Auth.API.Interfaces;
using System.Security.Authentication;

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

public class LoginHandler(IAuthRepository repository,
    IUserRepository userRepository,
    IJwtTokenService jwtTokenService)
    : IRequestHandler<LoginCommand, LoginResult>
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
