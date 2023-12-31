using System.Diagnostics.Metrics;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Shared;

public class DatabaseContext : DbContext
{
    public DbSet<Measurement> Measurements { get; set; }
    public DbSet<ChargingDBSchedule> ChargingDBSchedule { get; set; }
    public DbSet<WallCharger> WallCharger { get; set; }
    public DbSet<TruckType> TruckType { get; set; }
    
    public DbSet<Settings> Settings { get; set; }
    
    public DbSet<SystemStatus> SystemStatus { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source = ../EfcDataAccess/Database.db");
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);  
        

    }
    
}