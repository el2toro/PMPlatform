using System.Security.Claims;
using Yarp.ReverseProxy.Transforms;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

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

builder.Services.AddCors(options =>
{
    options.AddPolicy("PMPolicy",
        policy => policy
            .WithOrigins("http://localhost:4200") // Angular app URL
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});

builder.Services.AddAuthorization();

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"))
    .AddTransforms(builderContext =>
    {
        builderContext.AddRequestTransform((context) =>
        {
            //TODO: use an extension to add headers
            var user = context.HttpContext.User;

            if (user.Identity?.IsAuthenticated == true)
            {
                var tenantId = user.FindFirst("tenantId")?.Value;
                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (!string.IsNullOrEmpty(tenantId))
                    context.ProxyRequest.Headers.Add("X-Tenant-Id", tenantId);

                if (!string.IsNullOrEmpty(userId))
                    context.ProxyRequest.Headers.Add("X-User-Id", userId);
            }

            return ValueTask.CompletedTask;
        });
    });

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseCors("PMPolicy");
app.UseAuthentication();
app.UseAuthentication();

app.MapReverseProxy();

app.MapGet("/", () => "This is working");

app.Run();

