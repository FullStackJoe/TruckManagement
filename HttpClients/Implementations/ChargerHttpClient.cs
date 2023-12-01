using System.Net.Http.Json;
using HttpClients.ClientInterface;
using WebApplication1.Shared;

namespace HttpClients.Implementations;

public class ChargerHttpClient : IChargerService
{
    private readonly HttpClient httpClient;
    
    public ChargerHttpClient(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }
    public async Task<WallCharger> CreateAsync(WallCharger newCharger)
    {
        try
        {
            // NOT PRODUCTION READY
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
            var httpClient = new HttpClient(handler);
            
            HttpResponseMessage response = await httpClient.PostAsJsonAsync("https://localhost:7224/charger", newCharger);

            if (response.IsSuccessStatusCode)
            {
                // Deserialize and return the WallCharger object from the response
                WallCharger createdCharger = await response.Content.ReadFromJsonAsync<WallCharger>();
                return createdCharger;
            }
            else
            {
                // Handle error
                string content = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error: {content}");
                return null; // or handle differently based on your error handling strategy
            }
        }
        catch (Exception ex)
        {
            // Handle exception
            Console.WriteLine($"Exception: {ex.Message}");
            return null; // or handle differently based on your error handling strategy
        }
    }
}