namespace Project.API.Project.GetProjects;

public class GetProjectsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("tenants/{tenantId:guid}/projects",
            async ([FromRoute] Guid tenantId, [AsParameters] PaginationRequest request, [FromServices] ISender sender) =>
        {
            var result = await sender.Send(new GetProjectsQuery(tenantId, request.PageNumber, request.PageSize));
            return Results.Ok(result.PaginatedResponse);
        })
        .WithTags("Projects")
        .WithName("GetProjects")
        .Produces<PaginatedResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status401Unauthorized)
        .Produces(StatusCodes.Status403Forbidden);
        // .RequireAuthorization();
    }
}
