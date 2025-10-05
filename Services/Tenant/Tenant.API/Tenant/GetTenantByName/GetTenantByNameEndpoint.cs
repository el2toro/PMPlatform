
namespace Tenant.API.Tenant.GetTenantByName;

public class GetTenantByNameEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/tenants/{tenantName}", async (string tenantName, ISender sender) =>
        {
            var result = await sender.Send(new GetTenantByNameQuery(tenantName));

            //TODO:  input validation

            return Results.Ok(result.Tenant);
        });
    }
}
