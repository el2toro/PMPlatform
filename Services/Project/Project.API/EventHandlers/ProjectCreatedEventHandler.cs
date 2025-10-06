using Core.Messaging.Events;
using Mapster;
using MassTransit;
using Project.API.Dtos;
using Project.API.Hubs;

namespace Project.API.EventHandlers;

public class ProjectCreatedEventHandler
    (ILogger<ProjectCreatedEventHandler> logger,
    IProjectHub projectHub)
    : IConsumer<ProjectCreatedEvent>
{
    public async Task Consume(ConsumeContext<ProjectCreatedEvent> context)
    {
        logger.LogInformation("Project Event Handler: {ProjectCreatedEvent}", context.Message.GetType().Name);
        var projectDto = context.Message.Adapt<ProjectDto>();

        await projectHub.SendProject(projectDto);
    }
}
