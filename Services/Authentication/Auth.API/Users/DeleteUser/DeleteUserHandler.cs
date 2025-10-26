namespace Auth.API.Users.DeleteUser;

public record DeleteUserCommand(Guid UserId, Guid TenantId) : ICommand<DeleteUserResult>;
public record DeleteUserResult(bool IsSuccess);

public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required");
        RuleFor(x => x.TenantId).NotEmpty().WithMessage("TenantId is required");
    }
}

public class DeleteUserHandler(IUserRepository userRepository)
    : ICommandHandler<DeleteUserCommand, DeleteUserResult>
{
    public async Task<DeleteUserResult> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        await userRepository.DeleteUser(command.TenantId, command.UserId, cancellationToken);
        await userRepository.DeleteUserFromTenant(command.TenantId, command.UserId, cancellationToken);

        return new DeleteUserResult(true);
    }
}

