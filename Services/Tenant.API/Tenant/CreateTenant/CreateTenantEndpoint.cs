using Tenant.API.Dtos;

namespace Tenant.API.Tenant.CreateTenant;

//public record CreateTenantRequest(string Name, string Description);
//public record CreateTenantResponse();

public class CreateTenantEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/tenants", async (TenantDto request, ISender sender) =>
        {
            // var command = request.Adapt<CreateTenantCommand>();
            var result = await sender.Send(new CreateTenantCommand(request));
            return Results.Created($"api/tenants", result.TenantDto);
        });
    }
}
