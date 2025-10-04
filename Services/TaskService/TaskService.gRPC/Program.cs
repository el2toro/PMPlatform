var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc()
    .AddJsonTranscoding(); // Enables REST/HTTP calls

builder.Services.AddGrpcReflection(); // optional: for dev/testing

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

// Enable gRPC-Web
app.UseGrpcWeb(); // Must be called before MapGrpcService

// Configure the HTTP request pipeline.
app.MapGrpcService<TaskServiceImpl>().EnableGrpcWeb();
app.MapGrpcReflectionService().EnableGrpcWeb();

app.Run();
