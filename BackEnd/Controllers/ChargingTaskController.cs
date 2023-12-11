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
            
            // Retrieve the deadline hour from the database and convert to DateTimeOffset
            int deadlineHour = await dao.GetDailyDeadlineHour();
            DateTimeOffset deadline = chargingCalculation.IntDeadlineTotimeStamp(deadlineHour);
            
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
            
           return null;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    
}