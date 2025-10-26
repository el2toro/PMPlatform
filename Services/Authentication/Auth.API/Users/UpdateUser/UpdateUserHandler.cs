namespace Auth.API.Users.UpdateUser;

public record UpdateUserCommand(UserDto User) : ICommand<UpdateUserResult>;
public record UpdateUserResult(UserDto User);

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.User.Id).NotEmpty().WithMessage("Id is required");
        RuleFor(x => x.User.Email).NotEmpty().WithMessage("Email is required");
        RuleFor(x => x.User.FirstName).NotEmpty().WithMessage("FirstName is required");
        RuleFor(x => x.User.LastName).NotEmpty().WithMessage("LastName is required");
    }
}

public class UpdateUserHandler(IUserRepository userRepository)
    : ICommandHandler<UpdateUserCommand, UpdateUserResult>
{
    public async Task<UpdateUserResult> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        var existingUser = await userRepository.GetUserById(command.User.TenantId, command.User.Id, cancellationToken)
            ?? throw new UserNotFoundException(command.User.Id.ToString());

        existingUser.FirstName = command.User.FirstName;
        existingUser.LastName = command.User.LastName;
        existingUser.Email = command.User.Email;
        existingUser.UpdatedAt = DateTime.UtcNow;

        var updatedUser = await userRepository.UpdateUsersAsync(command.User.TenantId, existingUser, cancellationToken);
        var result = updatedUser.Adapt<UserDto>();

        return new UpdateUserResult(result);
    }
}
