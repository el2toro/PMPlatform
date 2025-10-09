using Board.Application.BoardHandlers.Queries.GetBoardById;
using Board.Application.Interfaces;
using Board.Infrastructure.Persistance;
using Board.Infrastructure.Repositories;
using Carter;
using Core.Behaviors;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IBoardRepository, BoardRepository>();
builder.Services.AddCarter();

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(typeof(GetBoardByIdHandler).Assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddDbContext<BoardDbContext>(config =>
{
    config.UseSqlServer(builder.Configuration.GetConnectionString("BoardDb"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.MapCarter();
app.UseExceptionHandler(option => { });

app.Run();

