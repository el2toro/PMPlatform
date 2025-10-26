namespace Auth.API.Users.CreateUser;

public record CreateUserCommand(UserDto User) : ICommand<CreateUserResult>;
public record CreateUserResult(UserDto User);

public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.User.Email).NotEmpty().WithMessage("Email is required");
        RuleFor(x => x.User.FirstName).NotEmpty().WithMessage("FirstName is required");
        RuleFor(x => x.User.LastName).NotEmpty().WithMessage("LastName is required");
    }
}

public class CreateUserHandler(IUserRepository userRepository,
    TenantAwareContextService tenantAwareContextService)
    : ICommandHandler<CreateUserCommand, CreateUserResult>
{
    public async Task<CreateUserResult> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var user = command.User.Adapt<User>();

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
        user.CreatedAt = DateTime.UtcNow;
        user.UpdatedAt = DateTime.UtcNow;

        Guid tenantId = tenantAwareContextService.TenantId;

        var createdUser = await userRepository.CreateUserAsync(tenantId, user, cancellationToken);
        var result = createdUser.Adapt<UserDto>();

        return new CreateUserResult(result);
    }
}
