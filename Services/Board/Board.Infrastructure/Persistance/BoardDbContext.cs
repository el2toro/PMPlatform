using Board.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Board.Infrastructure.Persistance;

public class BoardDbContext : DbContext
{
    public BoardDbContext(DbContextOptions<BoardDbContext> options) : base(options)
    {

    }

    public DbSet<Domain.Entities.Board> Boards { get; set; } = default!;
    public DbSet<Column> Columns { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Domain.Entities.Board>(entity =>
        {
            entity.HasKey(b => b.Id);
            entity.HasIndex(b => b.Id).IsUnique();
            entity.Property(b => b.Name).HasMaxLength(200);
            entity.Property(b => b.Description).HasMaxLength(1000);
        });

        modelBuilder.Entity<Column>(entity =>
        {
            entity.HasKey(b => b.Id);
            entity.HasIndex(b => b.Id).IsUnique();
            entity.Property(b => b.Name).HasMaxLength(200);

            entity.HasOne(c => c.Board)
            .WithMany(b => b.Columns)
            .HasForeignKey(b => b.BoardId);
        });
    }
}
