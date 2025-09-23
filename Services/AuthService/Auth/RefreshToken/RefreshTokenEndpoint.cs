namespace Auth.API.Auth.RefreshToken;

public record RefreshTokenRequest(string RefreshToken, Guid TenantId);
public record RefreshTokenResponse(string RefreshToken, string AccessToken);
public class RefreshTokenEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/auth/refreshtoken", async (RefreshTokenRequest request, ISender sender) =>
        {
            var command = request.Adapt<RefreshTokenCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<RefreshTokenResponse>();
            return Results.Ok(response);
        })
        .WithDisplayName("RefreshToken")
        .WithDescription("Refresh Token")
        .WithSummary("Refresh Token")
        .Produces<RefreshTokenResponse>();
    }
}
