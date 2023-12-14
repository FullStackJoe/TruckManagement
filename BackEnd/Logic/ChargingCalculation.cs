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

    // Teoretisk opladningstid - Tager ikke højde for langsom ladning efter 80%
    public async Task<int> GetChargingHours(ChargingTask chargingTask)
    {
        int percentage = chargingTask.BatteryPercentage;
        
        int batterySize = await GetBatterySizeAsync(chargingTask.TruckType);

        int chargerAmpere = await GetChargerAmpereAsync(chargingTask.ChargerId);

        int chargingHours = CalculateChargingHours(percentage, batterySize, chargerAmpere);
        
        return chargingHours;
    }
    
    // Static method to calculate the number of charging hours based on battery percentage, size, and charger ampere
    public static int CalculateChargingHours(int percentage, int batterySize, int chargerAmpere)
    {
        if (percentage < 0)
        {
            throw new ArgumentOutOfRangeException("percentage");
        } else if (percentage > 100)
        {
            throw new ArgumentOutOfRangeException("percentage");
        }
        else
        {
            // Calculate the charging hours and round up to the nearest whole number
            double tempResult = ((double)batterySize / chargerAmpere) / 100 * (100 - percentage);
            int chargingHours = (int)Math.Ceiling(tempResult);
            return chargingHours;
        }
    }
    
    // Private method to retrieve the battery size from the database
    private async Task<int> GetBatterySizeAsync(int truckId)
    {
        TruckType truckType = await _context.TruckType.SingleOrDefaultAsync(truck => truck.TruckTypeId == truckId);
        int result = truckType.BatterySizeAh;
        return result;
    }
    
    // Private method to retrieve the charger ampere rating from the database

    private async Task<int> GetChargerAmpereAsync(int chargerId)
    {
        WallCharger wallCharger = await _context.WallCharger.SingleOrDefaultAsync(charger => charger.ChargerId == chargerId);
        int result = wallCharger.ChargerAmpere;
        return result;
    }

    // Method to convert an integer hour to a DateTimeOffset representing a deadline
    public DateTimeOffset IntDeadlineTotimeStamp (int deadlineHour)
    {
        // Set charging deadline
        DateTime now = DateTime.Now;
        DateTime selectedDate;

        // Determine the deadline date based on the current time and the specified hour
        if (now.Hour < deadlineHour)
        {
            // If the current time is between midnight and 8 AM, set selectedDate to 8 AM of the same day
            selectedDate = new DateTime(now.Year, now.Month, now.Day, deadlineHour, 0, 0);
        }
        else
        {
            // If the current time is between 8 AM and midnight, set selectedDate to 8 AM of the next day
            DateTime tomorrow = now.AddDays(1);
            selectedDate = new DateTime(tomorrow.Year, tomorrow.Month, tomorrow.Day, deadlineHour, 0, 0);
        }
        // Create a TimeSpan for the offset (+01:00)
        TimeSpan offset = new TimeSpan(1, 0, 0);
        // Create a DateTimeOffset with the specified date, time, and offset
        DateTimeOffset deadline = new DateTimeOffset(selectedDate, offset);

        return deadline;
    }
    
}