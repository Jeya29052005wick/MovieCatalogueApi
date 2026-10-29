
using Microsoft.EntityFrameworkCore;
using MovieCatalogueApi.Models;

namespace MovieCatalogueApi.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Movie> Movies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.HasKey(m => m.Id);

            entity.Property(m => m.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(m => m.Genre)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(m => m.Language)
                .IsRequired()
                .HasMaxLength(100);

            entity.ToTable(table =>
            {
                table.HasCheckConstraint(
                    "CK_Movies_DurationMinutes",
                    "[DurationMinutes] >= 1 AND [DurationMinutes] <= 600");

                table.HasCheckConstraint(
                    "CK_Movies_ReleaseYear",
                    "[ReleaseYear] >= 1888 AND [ReleaseYear] <= 2100");
            });
        });
    }
}
