namespace Auth.API.Auth.Login;
public record LoginRequest(string Email, string Password);
public record LoginResponse(
        Guid UserId,
        Guid TenantId,
        string Email,
        string Token,
        string RefreshToken,
        IEnumerable<string> Roles);

public class LoginEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/auth/login", async (LoginRequest request, ISender sender) =>
        {
            var command = request.Adapt<LoginCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<LoginResponse>();
            return Results.Ok(response);
        })
        .WithDisplayName("Login")
        .WithDescription("Login")
        .WithSummary("Login")
        .Produces<LoginResponse>();
    }
}
