using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Shared;

public class DAO
{
    private readonly DatabaseContext context;

    public DAO(DatabaseContext context)
    {
        this.context = context;
    }
    
    public async Task<List<ChargingDBSchedule>> GetChargingDbSchedule()
    {
        List<ChargingDBSchedule> cheapestHours = await context.ChargingDBSchedule.ToListAsync();

        return cheapestHours;
    }
    public async void DeleteFirstChargingDbScheduleLine()
    {
        var firstLine = await context.ChargingDBSchedule.FirstOrDefaultAsync();

        if (firstLine != null)
        {
            context.ChargingDBSchedule.Remove(firstLine);
            await context.SaveChangesAsync();
        }

    }
}