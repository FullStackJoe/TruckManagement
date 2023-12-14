using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WebApplication1.Shared;

namespace WebApplication1.Controllers;

// ApiController attribute signifies this class is a controller with API-specific behaviors
[ApiController]
// Route attribute specifies the URL pattern for accessing the controller's actions
[Route("[controller]")]
public class ChargerController : ControllerBase
{
    // Database context for interacting with the database
    private readonly DatabaseContext _context;

    // Constructor to inject the database context dependency
    public ChargerController(DatabaseContext context)
    {
        _context = context;
    }

    // HTTP POST endpoint to create a new charger
    [HttpPost]
    public async Task<ActionResult<WallCharger>> CreateCharger([FromBody] WallCharger wallCharger)
    {
        try
        {
            // Add new WallCharger entity to the database asynchronously
            EntityEntry<WallCharger> added = await _context.WallCharger.AddAsync(wallCharger);
            // Save changes to the database
            await _context.SaveChangesAsync();

            // Return the added entity with a 200 OK response
            return added.Entity;
        }
        catch (Exception e)
        {
            // In case of an exception, log the error and return a 500 Internal Server Error
            // The error message is sent in the response body for debugging purposes
            // It's important to handle different types of exceptions and return appropriate status codes
            return StatusCode(500, e.Message); 
        }
    } 
}