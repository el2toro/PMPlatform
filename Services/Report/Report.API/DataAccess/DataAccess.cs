using Dapper;
using Microsoft.Data.SqlClient;
using Report.API.Dtos;

namespace Report.API.DataAccess;

public interface IDataAccess
{
    Task<AnaliticsDto> GetAnalytics(Guid tenantId);
    Task<ReportDto> GenerateReport(Guid tenantId);

}

public class DataAccess(IConfiguration configuration) : IDataAccess
{
    private AnaliticsDto analyticsDto = new AnaliticsDto();
    public async Task<AnaliticsDto> GetAnalytics(Guid tenantId)
    {
        try
        {
            await Task.WhenAll(
                GetProjectsAnalytics(),
                GetOverdueTasksCount(),
                GetProfessionals());
        }
        catch (Exception ex)
        {

            throw;
        }

        return analyticsDto;
    }

    public Task<ReportDto> GenerateReport(Guid tenantId)
    {
        throw new NotImplementedException();
    }

    private async Task GetProjectsAnalytics()
    {
        try
        {
            //TODO: to be reviewed, query based on tenantId
            using var connection = new SqlConnection(configuration.GetConnectionString("ProjectDb"));
            var projectQuery = "SELECT TOP 5 Name, EndDate FROM Projects AS Projects";
            analyticsDto.Projects = await connection.QueryAsync<ProjectDto>(projectQuery);

            var activePendingProjectsQuery = "SELECT (SELECT COUNT(*) FROM Projects WHERE ProjectStatus = 2) AS PendingProjects, (SELECT COUNT(*) FROM Projects WHERE ProjectStatus = 3) AS ActiveProjects, (SELECT COUNT(*) FROM Projects) AS TotalProjects";
            var activePendingProject = await connection.QueryAsync<ActivePendingTasks>(activePendingProjectsQuery);

            var pendingProjects = activePendingProject.FirstOrDefault()?.PendingProjects ?? 0;
            var activeProjects = activePendingProject.FirstOrDefault()?.ActiveProjects ?? 0;
            var totalProjects = activePendingProject.FirstOrDefault()?.TotalProjects ?? 0;

            analyticsDto.PendingProjects = pendingProjects;
            analyticsDto.ActiveProjects = activeProjects;
            analyticsDto.ProjectsPendingRate = (int)(((double)pendingProjects / totalProjects) * 100);
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    private async Task GetOverdueTasksCount()
    {

        try
        {
            using var taskConnection = new SqlConnection(configuration.GetConnectionString("TaskDb"));
            var overdueTasksCount = await taskConnection
                .QueryAsync<int>("SELECT COUNT(*) FROM Tasks WHERE CAST(DueDate AS DATE) < CAST(GETDATE() AS DATE)");

            var tasksCount = await taskConnection
                .QueryAsync<TasksCount>("SELECT (SELECT COUNT(*) FROM Tasks WHERE TaskStatus = 4) as ComplitedTasks, (SELECT COUNT(*) FROM Tasks) as TotalTasks");

            var totalTasks = tasksCount.FirstOrDefault()?.TotalTasks ?? 0;
            var complitedTasks = tasksCount.FirstOrDefault()?.ComplitedTasks ?? 0;

            analyticsDto.OverdueTasks = overdueTasksCount.FirstOrDefault();
            analyticsDto.ProjectCompletitionRate = (int)(((double)complitedTasks / totalTasks) * 100);
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    private async Task GetProfessionals()
    {
        try
        {
            using var authConnection = new SqlConnection(configuration.GetConnectionString("AuthenticationDb"));
            var professionals = await authConnection
                .QueryAsync<Professional>("SELECT FirstName AS Name FROM Users");

            var professionalCount = await authConnection
               .QueryAsync<int>("SELECT COUNT(*) AS Professionals FROM Users");

            analyticsDto.Professionals = professionalCount.FirstOrDefault();

            analyticsDto.ProfessionalsOfTheDay = professionals;
        }
        catch (Exception ex)
        {

            throw;
        }
    }
}
