using Carter;
using MediatR;


namespace Attachement.API.Endpoints;

public class AttachementEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/tenants/{tenantId}/projects/{projectId}/attachements/{attachementId}",
            (ISender sender) => "Hello from Attachement API!");

        app.MapPost("/tenants/{tenantId}/projects/{projectId}/attachements/",
           (ISender sender) => "Add Attachement API!");

        app.MapDelete("/tenants/{tenantId}/projects/{projectId}/attachements/{attachementId}",
           (ISender sender) => "Hello from Attachement API!");
    }
}