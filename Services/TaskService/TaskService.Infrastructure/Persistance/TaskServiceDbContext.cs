using TaskService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace TaskService.Infrastructure.Persistance;

public class TaskServiceDbContext : DbContext
{
    public TaskServiceDbContext(DbContextOptions<TaskServiceDbContext> options) : base(options) { }

    public DbSet<TaskItem> Tasks { get; set; }
    public DbSet<Subtask> Subtasks { get; set; }
    public DbSet<Comment> Commnets { get; set; }
    public DbSet<Attachment> Attachments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<TaskItem>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.Property(t => t.Title).HasMaxLength(100);
            entity.Property(t => t.Description).HasMaxLength(1000);

            entity.HasMany(t => t.Subtasks)
            .WithOne(st => st.Task)
            .HasForeignKey(st => st.TaskId);

            entity.HasMany(t => t.Comments)
            .WithOne(c => c.Task)
            .HasForeignKey(c => c.TaskId);

            entity.HasMany(t => t.Attachments)
            .WithOne(a => a.Task)
            .HasForeignKey(a => a.TaskId);
        });

        modelBuilder.Entity<Subtask>(entity =>
        {
            entity.HasKey(st => st.Id);
            entity.Property(st => st.Title).HasMaxLength(100);
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Content).HasMaxLength(1000);
        });

        modelBuilder.Entity<Attachment>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.Property(a => a.FileName).HasMaxLength(200);
            entity.Property(a => a.ContentType).HasMaxLength(100);
        });
    }
}
