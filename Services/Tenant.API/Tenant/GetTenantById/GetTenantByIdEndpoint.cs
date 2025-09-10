namespace Tenant.API.Tenant.GetTenantById;

public class GetTenantByIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/tenants/{tenantId:guid}", async (Guid tenantId, ISender sender) =>
        {
            var tenant = await sender.Send(new GetTenantByIdQuery(tenantId));

            if (tenant is null)
            {
                return Results.NotFound();
            }

            return Results.Ok(tenant);
        });
    }
}
