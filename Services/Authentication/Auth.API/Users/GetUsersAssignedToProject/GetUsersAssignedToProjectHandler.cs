namespace Auth.API.Users.GetUsersAssignedToProject;

public record GetUsersAssignedToProjectQuery(Guid TenantId, IEnumerable<Guid> UserIds) : IQuery<GetUsersAssignedToProjectResult>;
public record GetUsersAssignedToProjectResult(IEnumerable<UserDto> Users);

public class GetUsersAssignedToProjectQueryValidator : AbstractValidator<GetUsersAssignedToProjectQuery>
{
    public GetUsersAssignedToProjectQueryValidator()
    {
        RuleFor(x => x.TenantId).NotEmpty().WithMessage("TenantId is required");
        RuleFor(x => x.UserIds).NotEmpty().WithMessage("UserIds are required");
    }
}

public class GetUsersAssignedToProjectHandler(IUserRepository userRepository)
    : IQueryHandler<GetUsersAssignedToProjectQuery, GetUsersAssignedToProjectResult>
{
    public async Task<GetUsersAssignedToProjectResult> Handle(GetUsersAssignedToProjectQuery query, CancellationToken cancellationToken)
    {
        var users = await userRepository.GetUsersById(query.TenantId, query.UserIds, cancellationToken);
        var result = MapToDto(users);
        return new GetUsersAssignedToProjectResult(result);
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
