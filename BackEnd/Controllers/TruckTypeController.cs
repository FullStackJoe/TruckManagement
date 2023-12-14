using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using WebApplication1.Shared;

namespace WebApplication1.Controllers;

// ApiController attribute indicates this class is a Web API controller with HTTP API-specific behaviors
[ApiController]
// Route attribute defines the URL pattern for accessing the actions in this controller
[Route("[controller]")]
public class TruckTypeController : ControllerBase
{
    // Database context for interacting with the database
    private readonly DatabaseContext _context;

    // Constructor for injecting the database context dependency
    public TruckTypeController(DatabaseContext context)
    {
        _context = context;
    }
    
    // HTTP POST endpoint to create a new truck type
    [HttpPost]
    public async Task<ActionResult<TruckType>> CreateTruckType([FromBody] TruckType truckType)
    {
        try
        {
            // Add new TruckType entity to the database asynchronously
            EntityEntry<TruckType> added = await _context.TruckType.AddAsync(truckType);
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