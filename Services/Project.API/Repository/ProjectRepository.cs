using Microsoft.AspNetCore.Http.HttpResults;
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
    public async Task CreateProject(string name, string description, CancellationToken cancellationToken)
    {
        _dbContext.Projects.Add(new Models.Project
        {
            Name = name,
            Description = description,
            CreatedAt = DateTime.UtcNow,
            //CreatedBy = "system" // Placeholder, replace with actual user info
            TenantId = Guid.Parse("FF2C542E-5948-4726-A28A-4A5FD5CB76DA"), // Placeholder, replace with actual tenant info
            CreatedBy = Guid.Parse("00000000-0000-0000-0000-000000000001") // Placeholder, replace with actual user info           
        });
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<ProjectDto>> GetProjectsAsync()
    {
        return await _dbContext.Projects
            .Select(p => new ProjectDto(
                 p.Id,
                 p.Name,
                 p.Description,
                 p.CreatedAt,
                 p.CreatedBy,
                 p.TenantId
            )).ToListAsync();
    }
}
