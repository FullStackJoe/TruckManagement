using Microsoft.AspNetCore.Mvc;
using WebApplication1.Shared;
using WebApplication1.Shared.EnergyPrices;

namespace WebApplication1.Controllers;

[ApiController]
[Route("[controller]")]

public class ChargingTaskController : ControllerBase
{
    private readonly ChargingCalculation chargingCalculation;
    private readonly EnergyPrices energyPrices;

    public ChargingTaskController(ChargingCalculation chargingCalculation, EnergyPrices energyPrices)
    {
        this.chargingCalculation = chargingCalculation;
        this.energyPrices = energyPrices;
    }

    [HttpPost]
    public async Task<ActionResult<ChargingTask>> CreateChargingDBSchedule([FromBody]ChargingTask task)
    {
        try
        {
            int chargingTime = await chargingCalculation.CalculateChargingHours(task);
            Console.WriteLine(chargingTime);

            List<EnergyData> EnergiPriser = await energyPrices.getPriceDataAsync();
            Console.WriteLine(EnergiPriser);
            // Calculate charging time
            // Create chargingDBSchedules
            // Add chargingDBSchedules to DB
           // return Created($"/posts/{created.Id}", created);
           return null;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    /*
    [HttpPost]
    public async Task<ActionResult<Post>> CreateAsync([FromBody]PostCreationDto dto)
    {
        try
        {
            Post created = await postLogic.CreateAsync(dto);
            return Created($"/posts/{created.Id}", created);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    */
    
}