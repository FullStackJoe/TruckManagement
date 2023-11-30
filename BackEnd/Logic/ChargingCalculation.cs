using System.Collections;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Shared;

public class ChargingCalculation
{
    private readonly DatabaseContext _context;
    

    public ChargingCalculation(DatabaseContext context)
    {
        this._context = context;
    }
    
    public List<ChargingDBSchedule> CreateChargingDbSchedules(ChargingTask chargingTask)
    {
        return null;
    }

    // Teoretisk opladningstid - Tager ikke højde for langsom ladning efter 80%
    public async Task<int> CalculateChargingHours(ChargingTask chargingTask)
    {
        int percentage = chargingTask.BatteryPercentage;
        
        int batterySize = await GetBatterySizeAsync(chargingTask.TruckType);

        int chargerAmpere = await GetChargerAmpereAsync(chargingTask.ChargerId);
        
        double tempResult = ((double)batterySize / chargerAmpere) / 100 * (100 - percentage);
        int chargingHours = (int)Math.Ceiling(tempResult);
        
        return chargingHours;
    }
    
    public async Task<int> GetBatterySizeAsync(int truckId)
    {
        Truck truck = await _context.Truck.SingleOrDefaultAsync(truck => truck.TruckId == truckId);
        int result = truck.BatterySizeAh;
        return result;
    }
    
    public async Task<int> GetChargerAmpereAsync(int chargerId)
    {
        Charger charger = await _context.Charger.SingleOrDefaultAsync(charger => charger.ChargerId == chargerId);
        int result = charger.ChargerAmpere;
        return result;
    }
    
    /*private void UpdateCheapestHours()
    {
        DateTime selectedDate = new DateTime(2023, 11, 30, 2, 0, 0);
        // Create a TimeSpan for the offset (+01:00)
        TimeSpan offset = new TimeSpan(1, 0, 0);
        // Create a DateTimeOffset with the specified date, time, and offset
        DateTimeOffset deadline = new DateTimeOffset(selectedDate, offset);
        

        cheapestHours = myData
            .Where(x => x.TimeEnd <= deadline)
            .Where(x => x.TimeEnd > DateTimeOffset.Now)
            .Take(chargingHours)
            .OrderBy(x => x.TimeEnd)
            .ToList();
        
    } */
    
}