namespace Auth.API.Users.DeleteUser;

public class DeleteUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("tenants/{tenantId}/users/{userId}",
            async (Guid userId, Guid tenantId, ISender sender) =>
            {
                await sender.Send(new DeleteUserCommand(userId, tenantId));
                return Results.NoContent();
            });
    }
}

