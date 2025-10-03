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

app.Run();
