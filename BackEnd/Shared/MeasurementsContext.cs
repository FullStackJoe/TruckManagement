using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Shared;

public class MeasurementsContext : DbContext
{
    public DbSet<Measurement> Measurements { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source = ../BackEnd/Measurement.db");
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);  
        

    }
    
}