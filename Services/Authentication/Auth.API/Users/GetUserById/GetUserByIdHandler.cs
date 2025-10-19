using Auth.API.Interfaces;
using Core.CQRS;

namespace Auth.API.Users.GetUsersById;

public record GetUserByIdQuery(Guid TenantId, Guid UserId) : IQuery<GetUserByIdResult>;
public record GetUserByIdResult(UserDto User);

public class GetUserByIdHandler(IUserRepository userRepository)
    : IQueryHandler<GetUserByIdQuery, GetUserByIdResult>
{
    public async Task<GetUserByIdResult> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var users = await userRepository.GetUserById(request.TenantId, request.UserId, cancellationToken);
        var usersDtos = MapToDto(users);
        return new GetUserByIdResult(usersDtos);
    }

    //TODO: move to mapper, it is a duplicated method some is in GetUsersByTenantIdHandler
    private UserDto MapToDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            FullName = $"{user.FirstName} {user.LastName}"
        };
    }
}
