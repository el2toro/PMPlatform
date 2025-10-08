using Report.Worker;
using Report.Worker.Interfaces;
using Report.Worker.Services;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<Worker>();

builder.Services.AddScoped<IReportService, ReportService>();


var host = builder.Build();
host.Run();
