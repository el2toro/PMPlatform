namespace Project.API.Repository;

public class ProjectRepository(ProjectDbContext dbContext) : IProjectRepository
{
    private readonly ProjectDbContext _dbContext = dbContext;

    public async Task<Models.Project> CreateProjectAsync(Guid projectId, Models.Project project, CancellationToken cancellationToken)
    {
        var createdProject = _dbContext.Projects.Add(project).Entity;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return createdProject;
    }

    public async Task DeleteProjectAsync(Guid tenantdId, Guid projectId, CancellationToken cancellationToken)
    {
        var project = await _dbContext.Projects
            .FirstOrDefaultAsync(project => project.TenantId == tenantdId && project.Id == projectId);

        if (project is not null)
        {
            _dbContext.Projects.Remove(project);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task<Models.Project> GetProjectByIdAsync(Guid tenantId, Guid projectId, CancellationToken cancellationToken)
    {
        var project = await _dbContext.Projects
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == projectId && p.TenantId == tenantId, cancellationToken);

        return project ?? new Models.Project();
    }

    public async Task<(IEnumerable<Models.Project>, int)>
        GetProjectsAsync(Guid tenantId, int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var items = await _dbContext.Projects
            .AsNoTracking()
            .Where(p => p.TenantId == tenantId)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var totalCount = await _dbContext.Projects
            .Where(p => p.TenantId == tenantId)
            .CountAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<Models.Project> UpdateProjectAsync(ProjectDto projectDto, CancellationToken cancellationToken)
    {
        var existingProject = await _dbContext.Projects
            .FirstOrDefaultAsync(project => project.TenantId == projectDto.TenantId && project.Id == projectDto.Id, cancellationToken);

        projectDto.Adapt(existingProject);

        //TODO: move to handler/business logic
        existingProject.UpdatedAt = DateTime.UtcNow;
        existingProject.UpdatedBy = projectDto.CreatedBy;

        //var updatedProject = _dbContext.Projects.Update(existingProject);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return existingProject;
    }
}
