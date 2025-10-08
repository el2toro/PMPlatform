using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using Report.API.DataAccess;
using Report.API.ReportDocuments;

namespace Report.API.Endpoints;

public class ReportEndpoints : ICarterModule
{
    public record ReportRequest(string name, string description);
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("tenants/{tenantId}/analytics",
            async (Guid tenantId, [FromServices] IDataAccess dataAccess, ISender sender) =>
        {
            var result = await dataAccess.GetAnalytics(tenantId);
            return Results.Ok(result);
        });

        app.MapPost("tenants/{tenantId}/report",
            async ([FromRoute] Guid tenantId,
            [FromBody] ReportRequest resquest,
            [FromServices] IDataAccess dataAccess,
            ISender sender) =>
        {
            var analytics = await dataAccess.GetAnalytics(tenantId);
            QuestPDF.Settings.License = LicenseType.Community;

            var document = new ReportDocument(analytics, resquest);
            var pdf = document.GeneratePdf();

            // var result = await dataAccess.GenerateReport(tenantId);
            return Results.File(pdf, "application/pdf", $"report-{Guid.NewGuid}.pdf");
        });
    }
}
