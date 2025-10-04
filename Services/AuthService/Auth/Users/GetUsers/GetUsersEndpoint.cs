namespace Auth.API.Auth.Users.GetUsers;

public class GetUsersEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("users", async (ISender sender) =>
        {
            var result = await sender.Send(new GetUsersQuery());
            return Results.Ok(result.Users);
        });
    }
}
