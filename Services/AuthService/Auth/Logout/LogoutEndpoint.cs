namespace Auth.API.Auth.Logout;

public record LogoutRequest(string RefreshToken, Guid TenantId);
public record LogoutResponse(bool IsSuccess);

public class LogoutEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("logout", async (LogoutRequest request, ISender sender) =>
        {
            var command = request.Adapt<LogoutCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<LogoutResponse>();
            return Results.Ok(response);
        })
        .WithDisplayName("Logout")
        .WithDescription("Logout")
        .WithSummary("Logout")
        .Produces<LogoutResponse>();
    }
}
