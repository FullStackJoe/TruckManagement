using System.Net.Http.Json;
using HttpClients.ClientInterface;
using WebApplication1.Shared;

namespace HttpClients.Implementations;

public class ChargingTaskHttpClient : IChargingTaskService
{
    private readonly HttpClient httpClient;

    public ChargingTaskHttpClient(IHttpClientFactory clientFactory)
    {
        this.httpClient = clientFactory.CreateClient("BypassSSL");
    }
    
    public async Task CreateAsync(ChargingTask newChargingTask)
    {
        try
        {
            HttpResponseMessage response =
                await httpClient.PostAsJsonAsync("https://localhost:7224/ChargingTask", newChargingTask);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}