namespace Tenant.API.Tenant.CreateTenant;

public record CreateTenantRequest(string Name, string Description);
public record CreateTenantResponse();

public class CreateTenantEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/projects", async (CreateTenantRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateTenantCommand>();
            await sender.Send(command);
            return Results.Created("", new CreateTenantResponse());
        });
    }
}
