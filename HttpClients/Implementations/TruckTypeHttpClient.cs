using System.Net.Http.Json;
using HttpClients.ClientInterface;
using WebApplication1.Shared;

namespace HttpClients.Implementations;

public class TruckTypeHttpClient : ITruckTypeService
{
    private readonly HttpClient httpClient;
    
    public TruckTypeHttpClient(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }
    
    public async Task<TruckType> CreateAsync(TruckType newTruckType)
    {
        try
        {
            // NOT PRODUCTION READY: Bypass SSL certificate validation for development purposes
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
            var httpClient = new HttpClient(handler);
            
            HttpResponseMessage response = await httpClient.PostAsJsonAsync("https://localhost:7224/trucktype", newTruckType);

            if (response.IsSuccessStatusCode)
            {
                // Deserialize and return the WallCharger object from the response
                TruckType createdTruckType = await response.Content.ReadFromJsonAsync<TruckType>();
                return createdTruckType;
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