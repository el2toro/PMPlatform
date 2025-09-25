using Mapster;
using Microsoft.EntityFrameworkCore;
using Project.API.Data;
using Project.API.Dtos;

namespace Project.API.Repository;

public class ProjectRepository : IProjectRepository
{
    private readonly ProjectDbContext _dbContext;
    public ProjectRepository(ProjectDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Models.Project>
        CreateProjectAsync(string name,
        string? description,
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken)
    {
        var project = new Models.Project
        {
            Name = name,
            Description = description,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            ProjectStatus = Enums.ProjectStatus.NotStarted,
            StartDate = startDate,
            EndDate = endDate,
            TenantId = Guid.Parse("FF2C542E-5948-4726-A28A-4A5FD5CB76DA"), // Placeholder, replace with actual tenant info
            CreatedBy = Guid.Parse("3C484FF2-85DD-4A9B-989E-0C09FB3B8452") // Placeholder, replace with actual user info           
        };

        _dbContext.Projects.Add(project);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return project;
    }

    public async Task<ProjectDetailsDto> GetProjectDetailsAsync(Guid projectId, Guid tenantId, CancellationToken cancellationToken)
    {
        var project = await _dbContext.Projects
            .Include(p => p.Tasks)
            .AsNoTracking()
            .Where(p => p.Id == projectId && p.TenantId == tenantId)
            .Select(p => new ProjectDetailsDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                CreatedAt = p.CreatedAt,
                CreatedBy = p.CreatedBy,
                TenantId = p.TenantId,
                ProjectStatus = p.ProjectStatus,
                EndDate = p.EndDate,
                Tasks = p.Tasks.Select(t => new TaskItemDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    TaskStatus = t.TaskStatus,
                    AssignedTo = t.AssignedTo,
                    DueDate = t.DueDate,
                    CreatedAt = t.CreatedAt,
                    CreatedBy = t.CreatedBy,
                    ProjectId = t.ProjectId,
                    Subtasks = t.Subtasks.Select(st =>
                    new SubtaskDto(
                        st.Id,
                        st.TaskId,
                        st.Title,
                        st.IsCompleted
                    )).ToList(),
                    Comments = t.Comments.Select(c =>
                    new CommentDto
                    {
                        Id = c.Id,
                        Content = c.Content,
                        TaskId = c.TaskId,
                        CreatedAt = c.CreatedAt,
                        CreatedBy = c.UserId
                    }).ToList(),

                }).ToList()
            }).FirstOrDefaultAsync(cancellationToken);

        ArgumentNullException.ThrowIfNull(project, nameof(project));

        return project;
    }

    public async Task<IEnumerable<Models.Project>> GetProjectsAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Projects
            .Include(p => p.Tasks)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Models.Project> UpdateProjectAsync(ProjectDto projectDto, CancellationToken cancellationToken)
    {
        var existingProject = await _dbContext.Projects.FindAsync(projectDto.Id, cancellationToken);
        //TODO : handle not found
        ArgumentNullException.ThrowIfNull(existingProject, nameof(existingProject));

        projectDto.Adapt(existingProject);

        //TODO: move to handler/business logic
        existingProject.UpdatedAt = DateTime.UtcNow;
        existingProject.UpdatedBy = Guid.Parse("3C484FF2-85DD-4A9B-989E-0C09FB3B8452");

        //var updatedProject = _dbContext.Projects.Update(existingProject);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return existingProject;
    }
}
