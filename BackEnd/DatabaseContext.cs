using System.Diagnostics.Metrics;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Shared;

public class DatabaseContext : DbContext
{
    public DbSet<Measurement> Measurements { get; set; }
    public DbSet<ChargingDBSchedule> ChargingDBSchedule { get; set; }
    public DbSet<Charger> Charger { get; set; }
    public DbSet<Truck> Truck { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source = ../BackEnd/Database.db");
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);  
        

    }
    
}