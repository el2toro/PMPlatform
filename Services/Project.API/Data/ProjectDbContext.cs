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
    public DbSet<Board> Boards { get; set; } = default!;
    public DbSet<BoardColumn> BoardColumns { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure entity relationships and constraints here if needed
        // Project → Boards
        modelBuilder.Entity<Board>()
            .HasOne(b => b.Project)
            .WithMany(p => p.Boards)
            .HasForeignKey(b => b.ProjectId);


        // Board → Columns
        modelBuilder.Entity<BoardColumn>()
            .HasOne(c => c.Board)
            .WithMany(b => b.Columns)
            .HasForeignKey(c => c.BoardId);


        // TaskItem → Project
        modelBuilder.Entity<TaskItem>()
            .HasOne(t => t.Project)
            .WithMany(p => p.Tasks)
            .HasForeignKey(t => t.ProjectId)
            .OnDelete(DeleteBehavior.NoAction);


        // TaskItem → BoardColumn
        modelBuilder.Entity<TaskItem>()
            .HasOne(t => t.Column)
            .WithMany(c => c.Tasks)
            .HasForeignKey(t => t.ColumnId);

        // Subtask → TaskItem
        modelBuilder.Entity<Subtask>()
            .HasOne(s => s.Task)
            .WithMany(t => t.Subtasks)
            .HasForeignKey(s => s.TaskId);


        // Comment → TaskItem
        modelBuilder.Entity<Comment>()
            .HasOne(c => c.Task)
            .WithMany(t => t.Comments)
            .HasForeignKey(c => c.TaskId);


    }
}
