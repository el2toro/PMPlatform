using Core.Behaviors;
using Core.Exceptions.Handler;
using Core.Messaging.MassTransit;
using Project.API.EventHandlers;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IProjectHub, ProjectHub>();
builder.Services.AddScoped<TenantAwareContextService>();
builder.Services.AddSingleton<ICacheService, CacheService>();

builder.Services.AddDbContext<ProjectDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ProjectDb"));
});

builder.Services.AddHttpClient<UserServiceClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:5056/"); // replace with your service URL
});

builder.Services.AddHttpClient<TaskServiceClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:5057/"); // replace with your service URL
});

var assembly = typeof(Program).Assembly;

builder.Services.AddMediatR(cfg =>
{
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
    cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
    cfg.RegisterServicesFromAssembly(assembly);
});

builder.Services.AddCarter();

var eventHandlerAssemblies = new Assembly[]
{
    typeof(ProjectCreatedEventHandler).Assembly,
    typeof(ProjectUpdatedEventHandler).Assembly,
   // typeof(ProjectDeletedEventHandler).Assembly
};

builder.Services.AddMessageBroker(builder.Configuration, eventHandlerAssemblies);

builder.Services.AddSignalR();
builder.Services.AddExceptionHandler<CustomExceptioHandler>();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    options.InstanceName = "ProjectMicroservice";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapCarter();

app.MapHub<ProjectHub>("/hub/project");
app.UseExceptionHandler(option => { });

app.Run();

