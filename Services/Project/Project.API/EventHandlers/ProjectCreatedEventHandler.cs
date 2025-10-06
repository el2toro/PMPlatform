using Core.Messaging.Events;
using MassTransit;

namespace Project.API.EventHandlers;

public class ProjectCreatedEventHandler(ILogger<ProjectCreatedEventHandler> logger) : IConsumer<ProjectCreatedEvent>
{
    public Task Consume(ConsumeContext<ProjectCreatedEvent> context)
    {
        logger.LogInformation("Project Event Handler: {ProjectCreatedEvent}", context.Message.GetType().Name);
        var data = context.Message;
        throw new NotImplementedException();
    }
}
