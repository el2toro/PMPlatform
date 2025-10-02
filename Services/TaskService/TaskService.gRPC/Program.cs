using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TaskService.Application.Tasks.Commands.CreateTask;
using TaskService.Domain.Interfaces;
using TaskService.gRPC.Services;
using TaskService.Infrastructure.Persistance;
using TaskService.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();

builder.Services.AddScoped<ITaskServiceRepository, TaskServiceRepository>();

builder.Services.AddDbContext<TaskServiceDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("TaskDb"));
});

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(
        typeof(CreateTaskHandler).Assembly,
        Assembly.GetExecutingAssembly());// TaskService.gRPC itself for behaviors/pipelines

});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<TaskServiceImpl>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
