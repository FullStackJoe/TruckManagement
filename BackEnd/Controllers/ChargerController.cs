using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WebApplication1.Shared;

namespace WebApplication1.Controllers;

[ApiController]
[Route("[controller]")]

public class ChargerController : ControllerBase
{
    private readonly DatabaseContext _context;

    public ChargerController(ChargingCalculation chargingCalculation, DatabaseContext context)
    {
        _context = context;
    }
    
    [HttpPost]
    public async Task<ActionResult<WallCharger>> CreateCharger([FromBody] WallCharger wallCharger)
    {
        try
        {
            EntityEntry<WallCharger> added = await _context.WallCharger.AddAsync(wallCharger);
            await _context.SaveChangesAsync();
            return added.Entity;
        }
        catch (Exception e)
        {
            
            // Log the exception details here for debugging purposes
            return StatusCode(500, e.Message); // Consider using more specific status codes for different exceptions
        }
    } 
}