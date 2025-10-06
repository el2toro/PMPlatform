using Core.Messaging.Events;
using Mapster;
using MassTransit;
using MediatR;
using Project.API.Dtos;
using Project.API.Repository;

namespace Project.API.Project.CreateProject;

public record CreateProjectCommand(string Name, string? Description, DateTime StartDate, DateTime EndDate) : IRequest<CreateProjectResult>;
public record CreateProjectResult(ProjectDto ProjectDto);

public class CreateProjectHandler(IProjectRepository projectRepository, IPublishEndpoint publishEndpoint)
    : IRequestHandler<CreateProjectCommand, CreateProjectResult>
{
    public async Task<CreateProjectResult> Handle(CreateProjectCommand command, CancellationToken cancellationToken)
    {
        var project = await projectRepository
            .CreateProjectAsync(command.Name,
            command.Description,
            command.StartDate,
            command.EndDate,
            cancellationToken);

        var result = project.Adapt<ProjectDto>();

        await publishEndpoint.Publish<ProjectCreatedEvent>(result);

        return new CreateProjectResult(result);
    }
}
