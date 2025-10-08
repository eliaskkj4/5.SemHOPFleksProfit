using Microsoft.EntityFrameworkCore;
using FleksProfit.API.Models;

namespace FleksProfit.API.Data;

public class FleksProfitDbContext : DbContext
{
    public FleksProfitDbContext(DbContextOptions<FleksProfitDbContext> options)
        : base(options)
    {
    }

    public DbSet<SystemPerformance> SystemPerformances { get; set; }
    public DbSet<ElectricityPrice> ElectricityPrices { get; set; }
    public DbSet<ProfitCalculation> ProfitCalculations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<SystemPerformance>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Timestamp).IsRequired();
            entity.Property(e => e.Area).HasMaxLength(50);
        });

        modelBuilder.Entity<ElectricityPrice>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Timestamp).IsRequired();
            entity.Property(e => e.Area).HasMaxLength(50);
            entity.Property(e => e.Currency).HasMaxLength(10);
        });

        modelBuilder.Entity<ProfitCalculation>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CalculationDate).IsRequired();
            entity.Property(e => e.Area).HasMaxLength(50);
        });
    }
}
