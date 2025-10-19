using Auth.API.Exceptions;
using Auth.API.Interfaces;
using Core.CQRS;

namespace Auth.API.Users.UpdateUser;

public record UpdateUserCommand(UserDto User) : ICommand<UpdateUserResult>;
public record UpdateUserResult(UserDto User);

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
