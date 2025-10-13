using Core.Behaviors;
using Core.Exceptions.Handler;
using Core.Messaging.MassTransit;
using Core.Services;
using Microsoft.EntityFrameworkCore;
using TaskService.Application.Interfaces;
using TaskService.Application.Tasks.EventHandlers;
using TaskService.Infrastructure.Hubs;
using TaskService.Infrastructure.Persistance;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ITaskServiceRepository, TaskServiceRepository>();
builder.Services.AddScoped<ITaskServiceHub, TaskServiceHub>();
builder.Services.AddSingleton<ICacheService, CacheService>();

builder.Services.AddCarter();
//var assembly = typeof(Program).Assembly;

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(GetTaskByIdHandler).Assembly);

    //Congigure Mediator pre behavior (execute before reach the handle method)
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddDbContext<TaskServiceDbContext>(config =>
{
    config.UseSqlServer(builder.Configuration.GetConnectionString("TaskDb"));
});

builder.Services.AddMessageBroker(builder.Configuration, typeof(TaskCreatedEventHandler).Assembly);

builder.Services.AddSignalR();
builder.Services.AddExceptionHandler<CustomExceptioHandler>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "TaskMicroservice";
});

var app = builder.Build();

// Configure the HTTP request pipeline.

//app.UseHttpsRedirection();
app.MapCarter();

app.MapHub<TaskServiceHub>("/hub/notifications");
app.UseExceptionHandler(option => { });

app.Run();
