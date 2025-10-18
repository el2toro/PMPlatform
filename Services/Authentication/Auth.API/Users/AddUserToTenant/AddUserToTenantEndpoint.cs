namespace Auth.API.Users.AddUserToTenant;

public record AddUserToTenantRequest(Guid TenantId, Guid UserId, TenantRole Role);

public class AddUserToTenantEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/users/{userId}/tenants/{tenantId}",
            async (AddUserToTenantRequest request, ISender sender) =>
        {
            var command = request.Adapt<AddUserToTenantCommand>();
            await sender.Send(command);
            return Results.NoContent();
        });
    }
}
