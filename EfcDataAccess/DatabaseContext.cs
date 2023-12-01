using System.Diagnostics.Metrics;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Shared;

public class DatabaseContext : DbContext
{
    public DbSet<Measurement> Measurements { get; set; }
    public DbSet<ChargingDBSchedule> ChargingDBSchedule { get; set; }
    public DbSet<WallCharger> Charger { get; set; }
    public DbSet<TruckTypes> Truck { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source = ../EfcDataAccess/Database.db");
        optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);  
        

    }
    
}