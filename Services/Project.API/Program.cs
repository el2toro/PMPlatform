using Carter;
using Microsoft.EntityFrameworkCore;
using Project.API.Data;
using Project.API.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();

builder.Services.AddDbContext<ProjectDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ProjectDb"));
});

var assembly = typeof(Program).Assembly;

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

builder.Services.AddCarter();

var app = builder.Build();

app.MapGet("/ping", () => "pong");


// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.MapCarter();

app.Run();

