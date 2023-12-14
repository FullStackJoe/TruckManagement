using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Scaffolding.Metadata;
using WebApplication1.Shared.EnergyPrices;

namespace WebApplication1.Shared;

public class ShellyToggle
{
    private readonly HttpClient httpClient;
    private CancellationTokenSource cancellationTokenSource;
    private readonly IServiceScopeFactory scopeFactory;

    public ShellyToggle(IHttpClientFactory clientFactory, IServiceScopeFactory scopeFactory)
    {
        this.httpClient = clientFactory.CreateClient("BypassSSL");
        this.scopeFactory = scopeFactory;
    }

    public async void StartSystem()
    {
        // Inistitiancierer CancellationTokenSource
        cancellationTokenSource = new CancellationTokenSource();

        // Kører while loop som ny "thread"
        _ = Task.Run(async () => await RunScheduledTasks(cancellationTokenSource.Token));
    }

    private async Task RunScheduledTasks(CancellationToken cancellationToken)
    {
        using (var scope = scopeFactory.CreateScope())
        {
            var dao = scope.ServiceProvider.GetRequiredService<DAO>();

            // Empty the ChargingDBSchedule
            await dao.DeleteChargingDBSchedule();
            List<WallCharger> chargers = await dao.GetWallChargers();
            // Main loop der kører så længe systemer er aktivt

            while (!cancellationToken.IsCancellationRequested)
            {
                // Delay for at reducere belastningen af systemet
                await Task.Delay(400, cancellationToken); // Delay 0,6 second

                // For debugging purposes
                var currentTime = DateTimeOffset.Now;

                // create a new scope for each operation that involves database access. To make sure a new DBCotext is created 
                foreach (WallCharger charger in chargers)
                {
                    charger.SetHttpClient(httpClient);
                }

                // Hvis klokken er 1 minut over træder programmet ind i dette loop (Sker dermed en gang i timen)
                if (currentTime.Minute == 1)
                {
                    foreach (WallCharger charger in chargers)
                    {
                        // Chargingschedule for den givne oplader hentes
                        List<ChargingDBSchedule> cheapestHours = await dao.GetChargingDbSchedule(charger.ChargerId);

                        // Hvis der er opgaver for den givne opladaer
                        Console.Write(cheapestHours.Any());
                        if (cheapestHours.Any())
                        {
                            // Debugging purposes
                            Console.WriteLine("ID: " + charger.ChargerId + "Next Task: " +
                                              cheapestHours[0].TimeStart.Hour);

                            // Scenarier hvor opladeren skal tænde
                            if (currentTime.Hour == cheapestHours[0].TimeStart.Hour && (charger.ChargerState is OffState))
                            {
                                charger.TurnOn();
                                dao.DeleteFirstChargingDbScheduleLine(charger.ChargerId);
                            }
                            // Scnearier hvor opladeren skal forblive tændt
                            else if (currentTime.Hour == cheapestHours[0].TimeStart.Hour && (charger.ChargerState is OnState))
                            {
                                dao.DeleteFirstChargingDbScheduleLine(charger.ChargerId);
                            }
                            // Scnearier hvor opladeren skal slukke
                            else if (currentTime.Hour != cheapestHours[0].TimeStart.Hour && (charger.ChargerState is OnState))
                            {
                                charger.TurnOff();
                            }
                        }
                        // Hvis der ingen Chargingschedule for den givne oplader, slukkes.
                        else if (charger.ChargerState is OnState)
                        {
                            charger.TurnOff();
                        }
                        
                    }

                    // Vent 1 minut for at sikre loopet ikke køres før om en time
                    await Task.Delay(3000, cancellationToken);
                }
            }
        }

        Console.WriteLine("Program loop stopped");
    }

    public async void StopSystem()
    {
        // Annulerer while loopet
        cancellationTokenSource?.Cancel();

        // create a new scope for each operation that involves database access. To make sure a new DBCotext is created 
        using (var scope = scopeFactory.CreateScope())
        {
            var dao = scope.ServiceProvider.GetRequiredService<DAO>();

            // Henter alle ladere
            List<WallCharger> chargers = await dao.GetWallChargers();

            // Tænder alle ladere i chargers
            foreach (var charger in chargers)
            {
                charger.SetHttpClient(httpClient);
                charger.TurnOn();
            }
        }
    }
}