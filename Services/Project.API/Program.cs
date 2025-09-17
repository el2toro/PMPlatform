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

builder.Services.AddCors(options =>
{
    options.AddPolicy("PMPolicy",
        policy => policy
            .WithOrigins("http://localhost:4200") // Angular app URL
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});

var app = builder.Build();


// Configure the HTTP request pipeline.
app.UseCors("PMPolicy");
app.UseHttpsRedirection();
app.MapCarter();

app.Run();

