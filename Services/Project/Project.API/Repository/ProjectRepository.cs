namespace Project.API.Repository;

public class ProjectRepository(ProjectDbContext dbContext) : IProjectRepository
{
    private readonly ProjectDbContext _dbContext = dbContext;

    public async Task<Models.Project> CreateProjectAsync(Models.Project project, CancellationToken cancellationToken)
    {
        var createdProject = _dbContext.Projects.Add(project).Entity;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return createdProject;
    }

    public async Task<ProjectDetailsDto> GetProjectDetailsAsync(Guid projectId, Guid tenantId, CancellationToken cancellationToken)
    {
        var project = await _dbContext.Projects
            //   .Include(p => p.Tasks)
            .AsNoTracking()
            //   .Where(p => p.Id == projectId && p.TenantId == tenantId)
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
                //Tasks = p.Tasks.Select(t => new TaskItemDto
                //{
                //    Id = t.Id,
                //    Title = t.Title,
                //    Description = t.Description,
                //    TaskStatus = t.TaskStatus,
                //    AssignedTo = t.AssignedTo,
                //    DueDate = t.DueDate,
                //    CreatedAt = t.CreatedAt,
                //    CreatedBy = t.CreatedBy,
                //    ProjectId = t.ProjectId,
                //    Subtasks = t.Subtasks.Select(st =>
                //    new SubtaskDto(
                //        st.Id,
                //        st.TaskId,
                //        st.Title,
                //        st.IsCompleted
                //    )).ToList(),
                //    Comments = t.Comments.Select(c =>
                //    new CommentDto
                //    {
                //        Id = c.Id,
                //        Content = c.Content,
                //        TaskId = c.TaskId,
                //        CreatedAt = c.CreatedAt,
                //        CreatedBy = c.UserId
                //    }).ToList(),

                //}).ToList()
            }).FirstOrDefaultAsync(cancellationToken);

        ArgumentNullException.ThrowIfNull(project, nameof(project));

        return project;
    }

    public async Task<(IEnumerable<Models.Project>, int)> GetProjectsAsync(Guid tenantId, int pageNumber, int pageSize, CancellationToken cancellationToken)
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
