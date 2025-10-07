using Microsoft.EntityFrameworkCore;
using Auth.API.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AuthenticationDb")));

// Register JWT service
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddHttpClient<TenantServiceClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7090/"); // replace with your service URL
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

// Configure the HTTP request pipeline.
app.MapCarter();
app.UseHttpsRedirection();

app.Run();

