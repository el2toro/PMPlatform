namespace Auth.API.Users.GetUsersByTenantId;

public record GetUsersByTenantIdQuery(Guid TenantId) : IQuery<GetUsersByTenantIdResult>;
public record GetUsersByTenantIdResult(IEnumerable<UserDto> Users);

public class GetUsersByTenantIdQueryValidator : AbstractValidator<GetUsersByTenantIdQuery>
{
    public GetUsersByTenantIdQueryValidator()
    {
        RuleFor(x => x.TenantId).NotEmpty().WithMessage("TenantId is required");
    }
}
public class GetUsersByTenantIdHandler(IUserRepository userRepository)
    : IQueryHandler<GetUsersByTenantIdQuery, GetUsersByTenantIdResult>
{
    public async Task<GetUsersByTenantIdResult> Handle(GetUsersByTenantIdQuery request, CancellationToken cancellationToken)
    {
        var users = await userRepository.GetUsersByTenantId(request.TenantId, cancellationToken);
        var usersDtos = MapToDto(users);
        return new GetUsersByTenantIdResult(usersDtos);
    }

    //TODO: move to mapper, it is a duplicated method some is in GetUsersByIdHandler
    private IEnumerable<UserDto> MapToDto(IEnumerable<User> users)
    {
        return users.Select(user => new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user?.FirstName!,
            LastName = user?.LastName!,
            FullName = $"{user?.FirstName} {user?.LastName}"
        });
    }
}
