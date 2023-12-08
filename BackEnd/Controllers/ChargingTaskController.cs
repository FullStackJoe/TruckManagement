using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WebApplication1.Shared;
using WebApplication1.Shared.EnergyPrices;

namespace WebApplication1.Controllers;

[ApiController]
[Route("[controller]")]

public class ChargingTaskController : ControllerBase
{
    private readonly ChargingCalculation chargingCalculation;
    private readonly EnergyPrices energyPrices;
    private readonly DatabaseContext context;
    private readonly DAO dao;

    public ChargingTaskController(ChargingCalculation chargingCalculation, EnergyPrices energyPrices, DatabaseContext context, DAO dao)
    {
        this.chargingCalculation = chargingCalculation;
        this.energyPrices = energyPrices;
        this.context = context;
        this.dao = dao;
    }

    [HttpPost]
    public async Task<ActionResult<ChargingTask>> CreateChargingDBSchedule([FromBody]ChargingTask task)
    {

        int chargerId = task.ChargerId;

        try
        {
            // Calculate charging time
            int chargingTime = await chargingCalculation.GetChargingHours(task);
            
            // Retrieve the deadline hour from the database
            int deadlineHour = await dao.GetDailyDeadlineHour();
            
            // Set charging deadline
            DateTime now = DateTime.Now;
            DateTime selectedDate;

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
            
            // Fetch EnergyPrices
            List<EnergyData> fetchedEnergyPrices = await energyPrices.getPriceDataAsync();
            fetchedEnergyPrices = fetchedEnergyPrices.OrderBy(x => x.DKK_per_kWh).ToList();
            
            List<EnergyData> cheapestHours = fetchedEnergyPrices
                .Where(x => x.TimeEnd <= deadline)
                .Where(x => x.TimeEnd > DateTimeOffset.Now)
                .Take(chargingTime)
                .OrderBy(x => x.TimeEnd)
                .ToList();
            
            // Create chargingDBSchedules
            List<ChargingDBSchedule> schedule = cheapestHours
                .Select(x => new ChargingDBSchedule
                {
                    ChargerId = chargerId,
                    TimeStart = x.TimeStart.DateTime // Converting DateTimeOffset to DateTime
                })
                .ToList();
            

            // Add chargingDBSchedules to DB
            await context.ChargingDBSchedule.AddRangeAsync(schedule);
            await context.SaveChangesAsync();
            
           // return Created($"/posts/{created.Id}", created);
           return null;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
}