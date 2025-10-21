using Carter;
using Core.Exceptions.Handler;
using Core.Messaging.MassTransit;
using Notification.API.EventHandlers;
using Notification.API.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCarter();
builder.Services.AddScoped<EmailService>();


var assembly = typeof(Program).Assembly;
builder.Services.AddMediatR(config =>
{
    //TODO: add behaviors logging ??
    config.RegisterServicesFromAssembly(assembly);
});

var eventHandlerAssemblies = new Assembly[]
{
    typeof(TaskAssigneeChangedEventHandler).Assembly
};

builder.Services.AddMessageBroker(builder.Configuration, eventHandlerAssemblies);

builder.Services.AddExceptionHandler<CustomExceptioHandler>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapCarter();
app.UseExceptionHandler(aptions => { });

app.Run();
