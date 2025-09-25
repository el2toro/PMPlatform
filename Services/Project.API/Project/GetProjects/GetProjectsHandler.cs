using MediatR;
using Project.API.Dtos;
using Project.API.Enums;
using Project.API.Repository;
using Project.API.Services;

namespace Project.API.Project.GetProjects;

public record GetProjectsQuery(int PageNumber, int PageSize) : IRequest<GetProjectsResponse>;
public record GetProjectsResponse(PaginatedResponse PaginatedResponse);
public class GetProjectsHandler(IProjectRepository projectRepository, UserServiceClient userServiceClient)
    : IRequestHandler<GetProjectsQuery, GetProjectsResponse>
{
    public async Task<GetProjectsResponse> Handle(GetProjectsQuery query, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await projectRepository.GetProjectsAsync(query.PageNumber, query.PageSize, cancellationToken);

        //Parallel.ForEach(result, project =>
        //{
        //    // This is just to simulate some processing per project
        //    // In a real-world scenario, you might perform more complex operations here
        //    var progress = CalculateProgress(project.Tasks);
        //});

        var projects = new List<ProjectDto>();

        foreach (var project in items)
        {
            var newProject = new ProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                CreatedAt = project.CreatedAt,
                CreatedBy = project.CreatedBy,
                TenantId = project.TenantId,
                ProjectStatus = project.ProjectStatus,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                Progress = CalculateProgress(project.Tasks),
                //TODO: fetch actual team members
                Team = await GetTeam(project.TenantId)
            };

            projects.Add(newProject);
        };

        var response = new PaginatedResponse
        {
            PageNumber = query.PageNumber,
            PageSize = query.PageSize,
            TotalItems = totalCount,
            Items = projects
        };

        return new GetProjectsResponse(response);
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
        if (tasks.Count() == 0) return 0;

        if (tasks.All(t => t.TaskStatus == TaskItemStatus.Done)) return 100;

        tasks = tasks.Where(t => t.TaskStatus != TaskItemStatus.Cancelled);

        int totalTasks = tasks.Count();
        double sumTaskProgress = 0;

        foreach (var task in tasks)
        {
            int totalSubtasks = task.Subtasks.Count();
            int completedSubtasks = task.Subtasks.Count(st => st.IsCompleted);

            double taskProgress = totalSubtasks > 0
                ? ((double)completedSubtasks / totalSubtasks)
                : (task.TaskStatus == TaskItemStatus.Done ? 1 : 0);

            sumTaskProgress += taskProgress;
        }


        return totalTasks > 0
            ? (int)(100 * (sumTaskProgress / totalTasks))
            : 0;
    }
}
