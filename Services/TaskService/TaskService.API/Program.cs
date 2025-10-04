using Microsoft.EntityFrameworkCore;
using TaskService.Infrastructure.Persistance;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ITaskServiceRepository, TaskServiceRepository>();

builder.Services.AddCarter();
//var assembly = typeof(Program).Assembly;

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(GetTaskByIdHandler).Assembly);

    //Congigure Mediator pre behavior (execute before reach the handle method)
    //config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    // config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddDbContext<TaskServiceDbContext>(config =>
{
    config.UseSqlServer(builder.Configuration.GetConnectionString("TaskDb"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.MapCarter();

app.Run();
