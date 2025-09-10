namespace Auth.API.Auth.Register;

public record RegisterRequest(string FirstName, string LastName, string Email, string Password, string TenantName);
public record RegisterResponse(bool IsSuccess);
public class RegisterEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/auth/register", async (RegisterRequest request, ISender sender) =>
        {
            var command = request.Adapt<RegisterCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<RegisterResponse>();
            return Results.Created();
        })
        .WithDisplayName("Register")
        .WithDescription("Register")
        .WithSummary("Register")
        .Produces<RegisterResponse>();
    }
}
