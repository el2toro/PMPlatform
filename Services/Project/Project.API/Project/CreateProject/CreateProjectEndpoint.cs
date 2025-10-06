namespace Project.API.Project.CreateProject;

public record CreateProjectRequest(string Name, string? Description, DateTime StartDate, DateTime EndDate);

public class CreateProjectEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("projects",
            async (CreateProjectRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateProjectCommand>();
            var response = await sender.Send(command);

            return Results.Created($"/projects/{response.ProjectDto.Id}", response.ProjectDto);
        });
    }
}
