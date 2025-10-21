using Core.Messaging.Events.Task;
using MassTransit;
using Notification.API.Services;

namespace Notification.API.EventHandlers;

public class TaskAssigneeChangedEventHandler(EmailService emailService)
    : IConsumer<TaskAssigneeChangedEvent>
{
    public async Task Consume(ConsumeContext<TaskAssigneeChangedEvent> context)
    {

        string to = "application.service001@gmail.com";
        string subject = "New task assigned";
        string body = $"You have a new task to do: Task id: {context.Message.Id}, Task name: {context.Message.Title}";
        await emailService.SendEmailAsync(to, subject, body);

    }
}
