using Mapster;
using MassTransit;
using Project.API.Dtos;
using Project.API.Hubs;
using Project.API.Project.UpdateProject;

namespace Project.API.EventHandlers;

public class ProjectUpdatedEventHandler(ILogger<ProjectUpdatedEventHandler> logger, IProjectHub projectHub)
    : IConsumer<UpdateProjectEndpoint>
{
    public async Task Consume(ConsumeContext<UpdateProjectEndpoint> context)
    {
        logger.LogInformation("Project Event Handler: {ProjectUpdatedEvent}", context.Message.GetType().Name);
        var result = context.Message.Adapt<ProjectDto>();
        await projectHub.SendUpdatedProject(result);
    }
}
