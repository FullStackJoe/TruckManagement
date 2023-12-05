using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WebApplication1.Shared;
namespace WebApplication1.Controllers;

[ApiController]
[Route("[controller]")]
public class TruckTypeController : ControllerBase
{
    private readonly DatabaseContext _context;

    public TruckTypeController(DatabaseContext context)
    {
        _context = context;
    }
    
    [HttpPost]
    public async Task<ActionResult<TruckType>> CreateTruckType([FromBody] TruckType truckType)
    {
        try
        {
            EntityEntry<TruckType> added = await _context.TruckType.AddAsync(truckType);
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