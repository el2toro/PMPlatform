namespace Auth.API.Users.UpdateUser;

public class UpdateUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("tenants/{tenantId}/users", async (Guid tenantId, UserDto request, ISender sender) =>
        {
            var result = await sender.Send(new UpdateUserCommand(request));
            return Results.Ok(result.User);
        });
    }
}
