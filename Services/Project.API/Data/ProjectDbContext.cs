using Microsoft.EntityFrameworkCore;
using Project.API.Models;

namespace Project.API.Data;

public class ProjectDbContext : DbContext
{
    public ProjectDbContext(DbContextOptions<ProjectDbContext> options) : base(options)
    {
    }

    public DbSet<Models.Project> Projects { get; set; } = default!;
    public DbSet<TaskItem> Tasks { get; set; } = default!;
    public DbSet<Subtask> Subtasks { get; set; } = default!;
    public DbSet<Comment> Comments { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure entity relationships and constraints here if needed
        // Project
        modelBuilder.Entity<Models.Project>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(200);
            entity.Property(p => p.Description).HasMaxLength(1000);

            entity.HasMany(p => p.Tasks)
                  .WithOne(t => t.Project)
                  .HasForeignKey(t => t.ProjectId);
        });

        // TaskItem
        modelBuilder.Entity<TaskItem>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Title).IsRequired().HasMaxLength(200);

            entity.HasMany(t => t.Subtasks)
                  .WithOne(s => s.Task)
                  .HasForeignKey(s => s.TaskId);

            entity.HasMany(t => t.Comments)
                  .WithOne(c => c.Task)
                  .HasForeignKey(c => c.TaskId);
        });

        // Subtask
        modelBuilder.Entity<Subtask>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Title).IsRequired().HasMaxLength(200);
        });

        // Comment
        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Content).IsRequired().HasMaxLength(2000);
        });
    }
}
