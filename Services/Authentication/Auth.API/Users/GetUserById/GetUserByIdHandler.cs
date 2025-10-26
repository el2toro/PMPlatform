namespace Auth.API.Users.GetUsersById;

public record GetUserByIdQuery(Guid TenantId, Guid UserId) : IQuery<GetUserByIdResult>;
public record GetUserByIdResult(UserDto User);

public class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("UserId is required");
        RuleFor(x => x.TenantId).NotEmpty().WithMessage("TenantId is required");
    }
}

public class GetUserByIdHandler(IUserRepository userRepository)
    : IQueryHandler<GetUserByIdQuery, GetUserByIdResult>
{
    public async Task<GetUserByIdResult> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUserById(request.TenantId, request.UserId, cancellationToken);
        var userDtos = MapToDto(user);
        return new GetUserByIdResult(userDtos);
    }

    //TODO: move to mapper, it is a duplicated method some is in GetUsersByTenantIdHandler
    private UserDto MapToDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user?.FirstName!,
            LastName = user?.LastName!,
            FullName = $"{user?.FirstName!} {user?.LastName!}"
        };
    }
}
