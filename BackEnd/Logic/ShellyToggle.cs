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
            int faketime = currentTime.Minute - 40;
            

            // if (currentTime.Minute == 1)
            if (currentTime.Second == 1)
            {
                // Get list of charger id's
                WallCharger kage1 = new WallCharger()
                {
                    ChargerId = 1,
                    ChargerAmpere = 40,
                    TurnOnUri = "https://hooks.nabu.casa/gAAAAABlZGbPN3FYr7t3-SEEkEJBfBL-FehdjcOT74tGfRaJp7cWNTPmNg_0YXWfMHSoWX8-CdCafslWUIe9RgYnHoaN4MiHLse3yStC1oqO2HERo2kGcAzwrlXwyevoFP0zbHQsRAqYpi_TDClJ4uKT9Ffum7Rnu46HTH-Ob_09pNlyPELYgFo=",
                    TurnOffUri = "https://hooks.nabu.casa/gAAAAABlZHKmhSx7jVn8MNfZ25YYHwdaIeDjICmUh7uq6zMT7HZlnCd7oCku2FHzfvKfDCkVLWXCXYkCffy-qidz1gH2C4aOJeXtseYe5Q_GJ_C5wT0CXyoDvUBjIXI1ZALJK4UJLm3fLsw_b3vLud70uBBeiMFwY_x32iK6zj0wfHbUOrLCob4=",
                };
                WallCharger kage2 = new WallCharger()
                {
                    ChargerId = 2,
                    ChargerAmpere = 40,
                    TurnOnUri = "https://hooks.nabu.casa/gAAAAABlbby0DqovMB7kL-xtiRt6ZLTA19k5w9MBw56k2-O5EPrHsuzOKSWFNvETKjg_HmJP2iq7zDE0Go3vgoETGACMehnPiAqRZmtPC8cx_rOhbyA34LiK_8ty4pAzYFgBI9WmRzCe6eVAdVEQikylRkeV9hAhkEk4FcaSjOcneN6P4FT5gA4=",
                    TurnOffUri = "https://hooks.nabu.casa/gAAAAABlbb0E-8uClPrYA9ePrO8Us8rKPRxHQPtjHpKrrV07dv11y3SufsyXQdhflRl0M89y4SgkNkxejfJ_0nUTQYoIE3TpF6nURkbMfq0PM-9p-2-Iul6hix1YKIv-PZD7jILpSYVgz2FFUSCav__x4owO09gjyfjm8Rz6e2mEyifuFdiPhdQ=",
                };
                List<WallCharger> chargers = new List<WallCharger> { kage1, kage2 };
                
                foreach (WallCharger charger in chargers)
                {
                    Console.WriteLine(charger.ChargerId);
                }
                    
                
                foreach (WallCharger charger in chargers)
                {
                    List<ChargingDBSchedule> cheapestHours = await dao.GetChargingDbSchedule(charger.ChargerId);
                    
                    Console.WriteLine("Next Task: " + cheapestHours[0].TimeStart.Hour);
                    Console.WriteLine("Faketime: " + faketime);
                    if (faketime == cheapestHours[0].TimeStart.Hour && !isCharging)
                    {
                        await TurnOnCharger(charger.TurnOnUri);
                        dao.DeleteFirstChargingDbScheduleLine(charger.ChargerId);
                        Console.WriteLine("Charger turned on");
                    } else if (faketime == cheapestHours[0].TimeStart.Hour && isCharging)
                    {
                        dao.DeleteFirstChargingDbScheduleLine(charger.ChargerId);
                    }
                    else if (faketime != cheapestHours[0].TimeStart.Hour && isCharging)
                    {
                        await TurnOffCharger(charger.TurnOffUri);
                        Console.WriteLine("Charger turned off");
                    }
                    Console.WriteLine(isCharging);
                    await Task.Delay(3000, cancellationToken);
                }
            }
                
        }
    }
    
    public void StopSystem()
    {
        cancellationTokenSource?.Cancel();
        // gO get all TurnOnUris
        // TurnOnCharger();
    }
    private async Task TurnOnCharger(string uri)
    {
        isCharging = true;
        try
        {
            var response = await httpClient.PostAsync(uri, new StringContent(""));
            // response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            // Console.WriteLine(ex);
        }
    }
    
    private async Task TurnOffCharger(string uri)
    {
        isCharging = false;
        try
        {
            var response = await httpClient.PostAsync(uri, new StringContent(""));
            // response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            // Console.WriteLine(ex);
        }
    }
}