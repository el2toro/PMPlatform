using Carter;
using Core.Behaviors;
using Hangfire;
using Report.API.BackgroundJobs;
using Report.API.DataAccess;
using Report.API.Hubs;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCarter();
//builder.Services.AddScoped<IReportServiceHub, ReportServiceHub>();
builder.Services.AddScoped<IDataAccess, DataAccess>();

builder.Services.AddHangfire(config =>
{
    config.UseSqlServerStorage(builder.Configuration.GetConnectionString("HangfireDb"),
        new Hangfire.SqlServer.SqlServerStorageOptions
        {
            CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
            SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
            QueuePollInterval = TimeSpan.Zero,
            UseRecommendedIsolationLevel = true,
            DisableGlobalLocks = true
        });
});

builder.Services.AddHangfireServer(); // Starts background job server

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

var app = builder.Build();


// Configure the HTTP request pipeline.
app.MapCarter();
app.UseHangfireDashboard("/hangfire"); // Accessible at /hangfire
app.MapHangfireDashboard(); // Optional in some setups

//app.UseHttpsRedirection();
//app.MapHub<ReportServiceHub>("/hub/report");
app.UseExceptionHandler(option => { });

// Schedule the job to call the API endpoint
RecurringJob.AddOrUpdate(() => AnalyticsService.RunAnalytics(), Cron.Minutely); // Adjust the schedule as needed

app.Run();

