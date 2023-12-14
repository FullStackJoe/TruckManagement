using System.Net.Http.Json;
using HttpClients.ClientInterface;
using WebApplication1.Shared;

namespace HttpClients.Implementations;

// This class implements IChargingTaskService and is responsible for handling HTTP requests related to ChargingTasks
public class ChargingTaskHttpClient : IChargingTaskService
{
    private readonly HttpClient httpClient;

    // Constructor that takes an IHttpClientFactory and creates an HttpClient
    public ChargingTaskHttpClient(IHttpClientFactory clientFactory)
    {
        // 'BypassSSL' is a named client configured to bypass SSL validation
        this.httpClient = clientFactory.CreateClient("BypassSSL");
    }
    
    // Method to create a new ChargingTask asynchronously
    public async Task CreateAsync(ChargingTask newChargingTask)
    {
        try
        {
            // Send a POST request to the specified URI with the newChargingTask object as JSON
            HttpResponseMessage response =
                await httpClient.PostAsJsonAsync("https://localhost:7224/ChargingTask", newChargingTask);

            // Ensure the response indicates success; otherwise, an exception will be thrown
            response.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            // Handle exceptions by logging the error
            Console.WriteLine(e);
            // Additional error handling logic can be added here
        }
    }
}