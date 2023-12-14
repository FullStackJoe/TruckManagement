using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WebApplication1.Shared;
using WebApplication1.Shared.EnergyPrices;

namespace WebApplication1.Controllers;

// ApiController attribute indicates this class is a Web API controller with HTTP API-specific behaviors
[ApiController]
// Route attribute defines the URL pattern for accessing the actions in this controller
[Route("[controller]")]
public class ChargingTaskController : ControllerBase
{
    // Dependencies injected through the constructor
    private readonly ChargingCalculation chargingCalculation;
    private readonly EnergyPrices energyPrices;
    private readonly DatabaseContext context;
    private readonly DAO dao;

    // Constructor for dependency injection
    public ChargingTaskController(ChargingCalculation chargingCalculation, EnergyPrices energyPrices, DatabaseContext context, DAO dao)
    {
        this.chargingCalculation = chargingCalculation;
        this.energyPrices = energyPrices;
        this.context = context;
        this.dao = dao;
    }

    // POST endpoint for creating a charging schedule
    [HttpPost]
    public async Task<ActionResult<ChargingTask>> CreateChargingDBSchedule([FromBody]ChargingTask task)
    {
        int chargerId = task.ChargerId;

        try
        {
            // Calculate the number of hours required for charging
            int chargingTime = await chargingCalculation.GetChargingHours(task);
            
            // Get the charging deadline hour and convert it to a timestamp
            int deadlineHour = await dao.GetDailyDeadlineHour();
            DateTimeOffset deadline = chargingCalculation.IntDeadlineTotimeStamp(deadlineHour);
            
            // Retrieve and sort energy prices by cost
            List<EnergyData> fetchedEnergyPrices = await energyPrices.getPriceDataAsync();
            fetchedEnergyPrices = fetchedEnergyPrices.OrderBy(x => x.DKK_per_kWh).ToList();
            
            // Filter and select the cheapest energy slots within the deadline
            List<EnergyData> cheapestHours = fetchedEnergyPrices
                .Where(x => x.TimeEnd <= deadline)
                .Where(x => x.TimeEnd > DateTimeOffset.Now)
                .Take(chargingTime)
                .OrderBy(x => x.TimeEnd)
                .ToList();
            
            // Create a list of charging schedules based on the cheapest hours
            List<ChargingDBSchedule> schedule = cheapestHours
                .Select(x => new ChargingDBSchedule
                {
                    ChargerId = chargerId,
                    TimeStart = x.TimeStart.DateTime // Convert DateTimeOffset to DateTime
                })
                .ToList();
            
            // Add the new charging schedules to the database and save changes
            await context.ChargingDBSchedule.AddRangeAsync(schedule);
            await context.SaveChangesAsync();
            
            return null; // Consider returning the created schedule or a successful response
        }
        catch (Exception e)
        {
            // Log the exception details and return a 500 Internal Server Error status
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
}
