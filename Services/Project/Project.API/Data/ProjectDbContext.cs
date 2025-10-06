using Microsoft.EntityFrameworkCore;
using Project.API.Models;

namespace Project.API.Data;

public class ProjectDbContext : DbContext
{
    public ProjectDbContext(DbContextOptions<ProjectDbContext> options) : base(options)
    {
    }

    public DbSet<Models.Project> Projects { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Models.Project>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.HasIndex(p => p.Id).IsUnique();
            entity.Property(p => p.Name).HasMaxLength(200);
            entity.Property(p => p.Description).HasMaxLength(1000);
            entity.Property(p => p.TenantId).IsRequired();
        });
    }
}
