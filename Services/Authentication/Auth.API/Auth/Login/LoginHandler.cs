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

public class LoginHandler(IAuthRepository authRepository,
    IUserRepository userRepository,
    IJwtTokenService jwtTokenService)
    : ICommandHandler<LoginCommand, LoginResult>
{
    public async Task<LoginResult> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserByEmail(command.Email, cancellationToken)
            ?? throw new UserNotFoundException(command.Email);

        if (!IsValidPassword(command.Password, user?.PasswordHash!))
            throw new InvalidCredentialException("Invalid Password");

        Guid tenantId = user!.UserTenants.FirstOrDefault()!.TenantId;
        TenantRole userRole = user.UserTenants.FirstOrDefault()!.Role;
        string token = jwtTokenService.GenerateToken(user, userRole.ToString(), tenantId);
        var refreshToken = jwtTokenService.GenerateRefreshToken(tenantId, user.Id);

        await authRepository.Login(refreshToken, cancellationToken);

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
            FirstName: user?.FirstName!,
            LastName: user?.LastName!,
            Token: token,
            RefreshToken: refreshToken.Token,
            Roles: new List<string> { role.ToString() }
        );
    }
}

public record GoogleLoginCommand(string Email, string Name) : ICommand<GoogleLoginResult>;
public record GoogleLoginResult(Guid UserId,
        Guid TenantId,
        string Email,
        string FirstName,
        string LastName,
        string Token,
        string RefreshToken,
        IEnumerable<string> Roles);

internal class GoogleLoginHandler(IAuthRepository repository,
    IUserRepository userRepository,
    IJwtTokenService jwtTokenService,
    TenantServiceClient tenantService)
    : ICommandHandler<GoogleLoginCommand, GoogleLoginResult>
{
    public async Task<GoogleLoginResult> Handle(GoogleLoginCommand command, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserByEmail(command.Email, cancellationToken);

        if (user is null)
        {
            user = new User
            {
                Email = command.Email,
                FirstName = command.Name,
                LastName = command.Name,
                PasswordHash = "default"
            };

            var tenant = await tenantService.GetTenantByName(command.Name);

            if (tenant is null)
            {
                //var stringBuilder = new StringBuilder();

                //while (tenant is null)
                //{
                //    int counter = 0;
                //    stringBuilder.Append(command.Name);
                //    stringBuilder.Append(" ");
                //    stringBuilder.Append(counter++);
                //    string tenantName = stringBuilder.ToString();
                //    tenant = await tenantService.GetTenantByName(tenantName);
                //}

                var tenantToBeCreated = new Tenant
                {
                    Name = command.Name,
                    Description = "New tenant for Google authentication"
                };

                tenant = await tenantService.CreateTenant(tenantToBeCreated);
            }

            await userRepository.CreateUserAsync(tenant.TenantId, user!, cancellationToken);
        }

        Guid tenantId = user.UserTenants.FirstOrDefault()!.TenantId;
        TenantRole userRole = user.UserTenants.FirstOrDefault()!.Role;
        string token = jwtTokenService.GenerateToken(user, userRole.ToString(), tenantId);
        var refreshToken = jwtTokenService.GenerateRefreshToken(tenantId, user.Id);

        await repository.Login(refreshToken, cancellationToken);

        var result = MapToDto(user, tenantId, token, refreshToken, userRole);

        return result;
    }

    private GoogleLoginResult MapToDto(User user, Guid tenantId, string token, Models.RefreshToken refreshToken, TenantRole role)
    {
        return new GoogleLoginResult(
            UserId: user.Id,
            TenantId: tenantId,
            Email: user.Email,
            FirstName: user?.FirstName!,
            LastName: user?.LastName!,
            Token: token,
            RefreshToken: refreshToken.Token,
            Roles: new List<string> { role.ToString() }
        );
    }
}
