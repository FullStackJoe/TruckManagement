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

        cancellationTokenSource = new CancellationTokenSource();
        _ = Task.Run(async () => await RunScheduledTasks(cancellationTokenSource.Token));
    }
    
    private async Task RunScheduledTasks(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            // await Task.Delay(1000 * 60 , cancellationToken); // Delay 1 minute
            await Task.Delay(600, cancellationToken); // Delay 0,8 second
            
            var currentTime = DateTimeOffset.Now;
            int faketime = currentTime.Minute - 10;
            // if (currentTime.Minute == 1)
            if (currentTime.Second == 1)
            {
                List<ChargingDBSchedule> cheapestHours = await dao.GetChargingDbSchedule();
                
                Console.WriteLine("Next Task: " + cheapestHours[0].TimeStart.Hour);
                Console.WriteLine("Faketime: " + faketime);
                if (faketime == cheapestHours[0].TimeStart.Hour && !isCharging)
                {
                    await TurnOnCharger();
                    dao.DeleteFirstChargingDbScheduleLine();
                    Console.WriteLine("Charger turned on");
                } else if (faketime == cheapestHours[0].TimeStart.Hour && isCharging)
                {
                    dao.DeleteFirstChargingDbScheduleLine();
                }
                else if (faketime != cheapestHours[0].TimeStart.Hour && isCharging)
                {
                    await TurnOffCharger();
                    Console.WriteLine("Charger turned off");
                }
                Console.WriteLine(isCharging);
                await Task.Delay(3000, cancellationToken);
            }
        }
    }
    
    public void StopSystem()
    {
        cancellationTokenSource?.Cancel();
        TurnOnCharger();
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