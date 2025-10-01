using Auth.API.Repository;

namespace Auth.API.Auth.Users.CreateUser;

public record CreateUserCommand(UserDto User) : IRequest<CreateUserResult>;
public record CreateUserResult(UserDto User);
public class CreateUserHandler(IUserRepository userRepository)
    : IRequestHandler<CreateUserCommand, CreateUserResult>
{
    public async Task<CreateUserResult> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var user = command.User.Adapt<User>();
        var createdUser = await userRepository.CreateUserAsync(user, cancellationToken);
        var result = createdUser.Adapt<UserDto>();
        return new CreateUserResult(result);
    }
}
