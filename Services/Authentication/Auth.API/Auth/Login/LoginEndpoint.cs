using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Auth.API.Auth.Login;

public record LoginRequest(string Email, string Password);
public record GoogleLoginRequest(string Credential);
public record LoginResponse(
        Guid UserId,
        Guid TenantId,
        string Email,
        string FirstName,
        string LastName,
        string Token,
        string RefreshToken,
        IEnumerable<string> Roles);

public class LoginEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("login", async (LoginRequest request, ISender sender) =>
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

        app.MapPost("login/google", async (GoogleLoginRequest request, ISender sender) =>
        {
            // request.Credential contains the Google JWT
            var payload = await GoogleJsonWebSignature.ValidateAsync(request.Credential);
            return Results.Ok(new
            {
                Email = payload.Email,
                Name = payload.Name,
                Picture = payload.Picture
            });
        })
     .WithDisplayName("LoginGoogle")
     .WithDescription("LoginGoogle")
     .WithSummary("Login Google")
     .Produces<LoginResponse>();
    }
}
