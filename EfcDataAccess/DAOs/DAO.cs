using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Shared;

public class DAO
{
    private readonly DatabaseContext context;

    public DAO(DatabaseContext context)
    {
        this.context = context;
    }
    
    public async Task<List<ChargingDBSchedule>> GetChargingDbSchedule(int id)
    {
        Console.WriteLine("STEP 1");
        // Use LINQ to filter the data before calling ToListAsync()
        List<ChargingDBSchedule> cheapestHours = await context.ChargingDBSchedule
            .Where(c => c.ChargerId == id)
            .ToListAsync();
        
        Console.WriteLine("STEP 2");
        return cheapestHours;
    }
    public async void DeleteFirstChargingDbScheduleLine(int chargerId)
    {
        var firstLine = await context.ChargingDBSchedule.FirstOrDefaultAsync(charger => charger.ChargerId == chargerId);

        if (firstLine != null)
        {
            context.ChargingDBSchedule.Remove(firstLine);
            await context.SaveChangesAsync();
        }

    }
    
    public async Task<List<WallCharger>> GetWallChargers()
    {
        List<WallCharger> wallChargers = await context.WallCharger.ToListAsync();
        return wallChargers;
        
    }

    public async Task<List<TruckType>> GetTruckTypes()
    {
        List<TruckType> truckTypes = await context.TruckType.ToListAsync();
        return truckTypes;
    }
}