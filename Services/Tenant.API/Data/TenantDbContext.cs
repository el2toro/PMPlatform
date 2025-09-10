using Microsoft.EntityFrameworkCore;

namespace Tenant.API.Data;

public class TenantDbContext : DbContext
{
    public TenantDbContext(DbContextOptions<TenantDbContext> options) : base(options)
    {
    }

    // DbSets for your entities
    public DbSet<Models.Tenant> Tenants { get; set; } = default!;
    public DbSet<Models.UserTenant> UserTenants { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure entity properties and relationships if needed
        modelBuilder.Entity<Models.Tenant>(entity =>
        {
            entity.HasKey(t => t.Id);

            entity.Property(t => t.Name)
                  .IsRequired()
                  .HasMaxLength(200);

            entity.HasIndex(t => t.Name)
                  .IsUnique();

            entity.Property(t => t.Description)
                  .HasMaxLength(1000);
        });

        modelBuilder.Entity<Models.UserTenant>(entity =>
        {
            entity.HasKey(ut => ut.Id);

            entity.HasIndex(ut => new { ut.UserId, ut.TenantId })
                  .IsUnique(); // user cannot join same tenant twice

            entity.Property(ut => ut.Role)
                  .IsRequired();

            //Relationship: UserTenant → Tenant
            entity.HasOne(ut => ut.Tenant)
                  .WithMany(t => t.UserTenants)
                  .HasForeignKey(ut => ut.TenantId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

    }
}
