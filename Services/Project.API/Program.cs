using Carter;
using Microsoft.EntityFrameworkCore;
using Project.API.Data;
using Project.API.Repository;
using Project.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<IBoardRepository, BoardRepository>();

builder.Services.AddDbContext<ProjectDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ProjectDb"));
});

builder.Services.AddHttpClient<UserServiceClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7194/"); // replace with your service URL
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

// Configure Authentication
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!))
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();


// Configure the HTTP request pipeline.
app.UseCors("PMPolicy");
app.UseHttpsRedirection();
app.MapCarter();

app.UseAuthentication();
app.UseAuthorization();

app.Run();

