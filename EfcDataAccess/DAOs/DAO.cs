using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Shared;

public class DAO
{
    private readonly DatabaseContext context;

    // Constructor to initialize the DatabaseContext
    public DAO(DatabaseContext context)
    {
        this.context = context;
    }
    
    // Retrieves a list of ChargingDBSchedule for a specific Charger ID
    public async Task<List<ChargingDBSchedule>> GetChargingDbSchedule(int id)
    {
        try
        {
            // use LINQ Filtering the ChargingDBSchedule by ChargerId and converting to List asynchronously
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
    
    // Deletes the first ChargingDBSchedule entry for a given charger ID
    public async void DeleteFirstChargingDbScheduleLine(int chargerId)
    {
        var firstLine = await context.ChargingDBSchedule.FirstOrDefaultAsync(charger => charger.ChargerId == chargerId);

        if (firstLine != null)
        {
            context.ChargingDBSchedule.Remove(firstLine);
            await context.SaveChangesAsync();
        }

    }
    
    // Updates the Settings in the database
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
                 // Adding new settings if  there is no row in the Settings table
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
    
    // Updates the SystemRunning status
    public async Task UpdateSystemRunning(bool systemStatus)
    {
        try
        {
            var firstLine = await context.SystemStatus.FirstOrDefaultAsync();
            SystemStatus newStatus = new SystemStatus()
            {
                Status = systemStatus
            };
            if (firstLine != null)
            {
                context.SystemStatus.Add(newStatus);
                context.SystemStatus.Remove(firstLine);
                
            }
            else
            {
                // Handling the case when there is no row in the Settings table
                context.SystemStatus.Add(newStatus);
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
    
    // Retrieves the current SystemStatus
    public async Task<SystemStatus> GetSystemStatus()
    {
        // Retrieve the first row from the "Settings" table
        var status = await context.SystemStatus.FirstOrDefaultAsync();
        return status;
    }
    
    // Retrieves the current Settings
    public async Task<Settings> GetSettings()
    {
        // Retrieve the first row from the "Settings" table
        var settings = await context.Settings.FirstOrDefaultAsync();
        return settings;
    }
    
    // Retrieves the Daily Deadline Hour from Settings
    public async Task<int> GetDailyDeadlineHour()
    {
        var settings = await context.Settings.FirstOrDefaultAsync();
        return settings?.DailyDeadline ?? 8; // Return a default value if settings is null
    }
    
    // Retrieves all WallChargers
    public async Task<List<WallCharger>> GetWallChargers()
    {
        List<WallCharger> wallChargers = await context.WallCharger.ToListAsync();
        return wallChargers;
        
    }
    
    // Deletes all ChargingDBSchedule entries
    public async Task DeleteChargingDBSchedule()
    {
        // Assuming _context is your database context
        var allChargingSchedules = await context.ChargingDBSchedule.ToListAsync();
        context.ChargingDBSchedule.RemoveRange(allChargingSchedules);
        await context.SaveChangesAsync();
    }

    // Retrieves all TruckTypes
    public async Task<List<TruckType>> GetTruckTypes()
    {
        List<TruckType> truckTypes = await context.TruckType.ToListAsync();
        return truckTypes;
    }
}