using System.Net.Http.Json;
using HttpClients.ClientInterface;
using WebApplication1.Shared;

namespace HttpClients.Implementations;

public class ChargingTaskHttpClient : IChargingTaskService
{
    private readonly HttpClient httpClient;

    public ChargingTaskHttpClient(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }
    
    public async void CreateAsync(ChargingTask newChargingTask)
    {
        
        // NOT PRODUCTION READY
        
        var handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
        var httpClient = new HttpClient(handler);
            
        HttpResponseMessage response = await httpClient.PostAsJsonAsync("https://localhost:7224/ChargingTask", newChargingTask);
        
    }
}