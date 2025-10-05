using Microsoft.EntityFrameworkCore;
using Tenant.API.Data;
using Tenant.API.Repository;
using Tenant.API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<ITenantRepository, TenantRepository>();
builder.Services.AddHttpClient<AuthServiceClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7194/"); // replace with your service URL
});


builder.Services.AddDbContext<TenantDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("TenantDb"));
});

var assembly = typeof(Program).Assembly;

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);

    //Congigure Mediator pre behavior (execute before reach the handle method)
    //config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    // config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddCarter();

var app = builder.Build();

// Configure the HTTP request pipeline

app.UseHttpsRedirection();
app.MapCarter();

app.Run();

