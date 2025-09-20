using MediatR;
using Project.API.Dtos;
using Project.API.Enums;
using Project.API.Repository;
using Project.API.Services;

namespace Project.API.Project.GetProjects;

public record GetProjectsQuery : IRequest<ProjectResponse>;
public record ProjectResponse(IEnumerable<ProjectDto> Projects);
public class GetProjectsHandler(IProjectRepository projectRepository, UserServiceClient userServiceClient)
    : IRequestHandler<GetProjectsQuery, ProjectResponse>
{
    public async Task<ProjectResponse> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
    {
        var result = await projectRepository.GetProjectsAsync(cancellationToken);

        //Parallel.ForEach(result, project =>
        //{
        //    // This is just to simulate some processing per project
        //    // In a real-world scenario, you might perform more complex operations here
        //    var progress = CalculateProgress(project.Tasks);
        //});

        var projects = new List<ProjectDto>();

        foreach (var project in result)
        {
            var newProject = new ProjectDto(
                   project.Id,
                   project.Name,
                   project.Description,
                   project.CreatedAt,
                   project.CreatedBy,
                   project.TenantId,
                   project.ProjectStatus,
                   project.EndDate,
                   project.StartDate,
                   CalculateProgress(project.Tasks),
                   //TODO: fetch actual team members
                   await GetTeam(project.TenantId));

            projects.Add(newProject);
        };

        //var projects = result.Select(p => new ProjectDto(
        //           p.Id,
        //           p.Name,
        //           p.Description,
        //           p.CreatedAt,
        //           p.CreatedBy,
        //           p.TenantId,
        //           p.ProjectStatus,
        //           p.EndDate,
        //           CalculateProgress(p.Tasks),
        //           //TODO: fetch actual team members
        //           GetTeam(p.TenantId)));

        return new ProjectResponse(projects);
    }

    private async Task<IEnumerable<UserDto>> GetTeam(Guid teanantId)
    {
        var data = await userServiceClient.GetUsersByTenantIdAsync(teanantId);

        return data.Take(1);
    }

    // Simple progress calculation based on task statuses
    // TODO: Refine this logic as per actual requirements
    private int CalculateProgress(IEnumerable<Models.TaskItem> tasks)
    {
        if (tasks.Count() is 0) return 0;

        if (tasks.All(t => t.TaskStatus == TaskItemStatus.Done)) return 100;

        // Count tasks that are not in Backlog, ToDo, or Cancelled status
        // TODO: Consider adding weights to different statuses for a more nuanced calculation
        int count = tasks.Where(t =>
        t.TaskStatus != TaskItemStatus.Backlog &&
        t.TaskStatus != TaskItemStatus.ToDo &&
        t.TaskStatus != TaskItemStatus.Cancelled).Count();

        return count > 0 ? (100 / count) : 100;

        // return (100 / count); //100% divided by number of incomplete tasks
    }
}
