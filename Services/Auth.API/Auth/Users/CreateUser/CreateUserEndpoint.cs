namespace Auth.API.Auth.Users.CreateUser;

public class CreateUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/tenants/{tenantId:guid}/users", async (UserDto request, ISender sender) =>
        {
            var result = await sender.Send(new CreateUserCommand(request));
            return Results.Created($"api/tenants/{"tenantId"}/users/{result.User.Id}", result.User);
        });
    }
}
