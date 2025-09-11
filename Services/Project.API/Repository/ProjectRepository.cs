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
    public async Task<Models.Project> CreateProjectAsync(string name, string description, CancellationToken cancellationToken)
    {
        var project = new Models.Project
        {
            Name = name,
            Description = description,
            CreatedAt = DateTime.UtcNow,
            //CreatedBy = "system" // Placeholder, replace with actual user info
            TenantId = Guid.Parse("FF2C542E-5948-4726-A28A-4A5FD5CB76DA"), // Placeholder, replace with actual tenant info
            CreatedBy = Guid.Parse("00000000-0000-0000-0000-000000000001") // Placeholder, replace with actual user info           
        };

        _dbContext.Projects.Add(project);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return project;
    }

    public async Task<IEnumerable<ProjectDto>> GetProjectsAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Projects
            .Select(p => new ProjectDto(
                 p.Id,
                 p.Name,
                 p.Description,
                 p.CreatedAt,
                 p.CreatedBy,
                 p.TenantId
            )).ToListAsync(cancellationToken);
    }
}
