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
        try
        {
            // Use LINQ to filter the data before calling ToListAsync()
            List<ChargingDBSchedule> cheapestHours = await context.ChargingDBSchedule
                .Where(c => c.ChargerId == id)
                .ToListAsync();
            
            return cheapestHours;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        return null;
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
    public async Task UpdateSettings(Settings newSettings)
    {
        try
        {
            var firstLine = await context.Settings.FirstOrDefaultAsync();
            
            if (firstLine != null)
            {
                context.Settings.Add(newSettings);
                context.Settings.Remove(firstLine);
                
            }
            else
            {
                // Handling the case when there is no row in the Settings table
                context.Settings.Add(newSettings);
                Console.WriteLine("Im inside a create function");
            }

            // Save the changes to the database
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    
    public async Task<Settings> GetSettings()
    {
        // Retrieve the first row from the "Settings" table
        var settings = await context.Settings.FirstOrDefaultAsync();
        return settings;
    }
    
    public async Task<int> GetDailyDeadlineHour()
    {
        var settings = await context.Settings.FirstOrDefaultAsync();
        return settings?.DailyDeadline ?? 8; // Return a default value if settings is null
    }
    
    public async Task<List<WallCharger>> GetWallChargers()
    {
        List<WallCharger> wallChargers = await context.WallCharger.ToListAsync();
        return wallChargers;
        
    }
    
    public async Task DeleteChargingDBSchedule()
    {
        // Assuming _context is your database context
        var allChargingSchedules = await context.ChargingDBSchedule.ToListAsync();
        context.ChargingDBSchedule.RemoveRange(allChargingSchedules);
        await context.SaveChangesAsync();
    }

    public async Task<List<TruckType>> GetTruckTypes()
    {
        List<TruckType> truckTypes = await context.TruckType.ToListAsync();
        return truckTypes;
    }
}