using Carter;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCarter();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapCarter();

app.Run();
