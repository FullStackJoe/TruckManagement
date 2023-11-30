using Microsoft.EntityFrameworkCore;
using WebApplication1.Shared.EnergyPrices;

namespace WebApplication1.Shared;

public class ShellyToggle
{
    private readonly HttpClient httpClient;
    private bool isCharging = false;
    private CancellationTokenSource cancellationTokenSource;
    private DAO dao;

    public ShellyToggle(HttpClient httpClient, DAO dao)
    {
        this.httpClient = httpClient;
        this.dao = dao;
    }
    
    public async void StartSystem()
    {
        Console.WriteLine("WORKS");
        List<ChargingDBSchedule> cheapestHours = await dao.GetChargingDbSchedule();

        cancellationTokenSource = new CancellationTokenSource();
        _ = Task.Run(async () => await RunScheduledTasks(cancellationTokenSource.Token, cheapestHours));
    }
    
    private async Task RunScheduledTasks(CancellationToken cancellationToken, List<ChargingDBSchedule> cheapestHours)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            // await Task.Delay(1000 * 60, cancellationToken); // Delay 1 minute
            await Task.Delay(900 , cancellationToken); // Delay 1 second
            var currentTime = DateTimeOffset.Now;
            int faketime = currentTime.Minute + 6;
            // if (currentTime.Minute == 1)
            if (1 == 1)
            {
                if (faketime == cheapestHours[0].TimeStart.Hour && !isCharging)
                {
                    await TurnOnCharger(); 
                    cheapestHours.RemoveAt(0);
                    Console.WriteLine("Charger turned on");
                } else if (faketime == cheapestHours[0].TimeStart.Hour && isCharging)
                {
                    await TurnOffCharger();
                    Console.WriteLine("Charger turned off");
                } else if (faketime + 1 > cheapestHours[0].TimeStart.Hour && isCharging)
                {
                    Console.WriteLine("I do nothing");
                }
                Console.WriteLine(faketime);
                Console.WriteLine(isCharging);
                
            }
        }
    }
    
    public void StopSystem()
    {
        cancellationTokenSource?.Cancel();
    }
    private async Task TurnOnCharger()
    {
        isCharging = true;
        try
        {
            var response = await httpClient.PostAsync("https://hooks.nabu.casa/gAAAAABlZGbPN3FYr7t3-SEEkEJBfBL-FehdjcOT74tGfRaJp7cWNTPmNg_0YXWfMHSoWX8-CdCafslWUIe9RgYnHoaN4MiHLse3yStC1oqO2HERo2kGcAzwrlXwyevoFP0zbHQsRAqYpi_TDClJ4uKT9Ffum7Rnu46HTH-Ob_09pNlyPELYgFo=", new StringContent(""));
            // response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            // Console.WriteLine(ex);
        }
    }
    
    private async Task TurnOffCharger()
    {
        isCharging = false;
        try
        {
            var response = await httpClient.PostAsync("https://hooks.nabu.casa/gAAAAABlZHKmhSx7jVn8MNfZ25YYHwdaIeDjICmUh7uq6zMT7HZlnCd7oCku2FHzfvKfDCkVLWXCXYkCffy-qidz1gH2C4aOJeXtseYe5Q_GJ_C5wT0CXyoDvUBjIXI1ZALJK4UJLm3fLsw_b3vLud70uBBeiMFwY_x32iK6zj0wfHbUOrLCob4=", new StringContent(""));
            // response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            // Console.WriteLine(ex);
        }
    }
}