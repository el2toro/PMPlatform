namespace Auth.API.Auth.Register;

public record RegisterRequest(string FirstName, string LastName, string Email, string Password, string TenantName);
public record RegisterResponse(RegisterResponseDto RegisterDto);
public class RegisterEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("register", async (RegisterRequest request, ISender sender) =>
        {
            var command = request.Adapt<RegisterCommand>();
            var result = await sender.Send(command);
            var response = result.Adapt<RegisterResponse>();
            return Results.Created($"api/auth/register/{result.RegisterDto.UserId}", response.RegisterDto);
        })
        .WithDisplayName("Register")
        .WithDescription("Register")
        .WithSummary("Register")
        .Produces<RegisterResponseDto>();
    }
}
