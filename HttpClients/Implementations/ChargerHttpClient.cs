using System.Net.Http.Json;
using HttpClients.ClientInterface;
using WebApplication1.Shared;

namespace HttpClients.Implementations;

public class ChargerHttpClient : IChargerService
{
    private readonly HttpClient httpClient;
    
    // Constructor to inject HttpClient dependency
    public ChargerHttpClient(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    // Method to create a new WallCharger asynchronously
    public async Task<WallCharger> CreateAsync(WallCharger newCharger)
    {
        try
        {
            // NOT PRODUCTION READY: Bypass SSL certificate validation for development purposes
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
            var httpClient = new HttpClient(handler);
            
            // Sending a POST request to the specified URI with the newCharger object as JSON
            HttpResponseMessage response = await httpClient.PostAsJsonAsync("https://localhost:7224/charger", newCharger);

            if (response.IsSuccessStatusCode)
            {
                // Deserialize the JSON response into a WallCharger object and return it
                WallCharger createdCharger = await response.Content.ReadFromJsonAsync<WallCharger>();
                return createdCharger;
            }
            else
            {
                // Log the error and return null (or handle the error based on your strategy)
                string content = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Error: {content}");
                return null;
            }
        }
        catch (Exception ex)
        {
            // Log the exception and return null (or handle differently based on your strategy)
            Console.WriteLine($"Exception: {ex.Message}");
            return null;
        }
    }
}