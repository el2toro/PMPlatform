using Microsoft.EntityFrameworkCore;
using Board.gRPC.Entities;

namespace Board.gRPC.DataContext;

public class BoardDbContext : DbContext
{
    public BoardDbContext(DbContextOptions<BoardDbContext> options) : base(options)
    {

    }

    public DbSet<Entities.Board> Boards { get; set; } = default!;
    public DbSet<Column> Columns { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Board
        modelBuilder.Entity<Entities.Board>(entiry =>
        {
            entiry.Property(b => b.Name)
            .HasMaxLength(200);

            entiry.Property(b => b.Description)
             .HasMaxLength(1000);
        });

        //Column
        modelBuilder.Entity<Entities.Column>(entity =>
        {
            entity.Property(c => c.Name)
            .HasMaxLength(200);

            entity.HasOne(c => c.Board)
             .WithMany(b => b.Columns)
             .HasForeignKey(c => c.BoardId);
        });
    }
}
