using System.Text.Json;

namespace WebApplication1.Shared.EnergyPrices;

public class EnergyPrices
{
    private readonly HttpClient _httpClient;
    // Store the current date in a specific format
    string currentDate = DateTime.Now.ToString("yyyy/MM-dd");
    // Store the date of the next day in a specific format
    string tomorrowDate = DateTime.Now.AddDays(1).ToString("yyyy/MM-dd");

    // Constructor to initialize the HttpClient
    public EnergyPrices(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Public method to get price data
    public async Task<List<EnergyData>> getPriceDataAsync()
    {
        // Load data for the current date
        List<EnergyData> data = await LoadDataAsync(currentDate);

        // If current time is after 1 PM, also load data for the next day
        if (IsAfterOnePM())
        {
            List<EnergyData> tomorrowData = await LoadDataAsync(tomorrowDate);
            
            // Filter out data where TimeEnd is before the current time
            data = data.Where(d => d.TimeEnd >= DateTimeOffset.Now).ToList();
            if (tomorrowData != null)
            {
                // Append tomorrow's data to today's list, if tomorrow's data is available
                data.AddRange(tomorrowData);
            }
        }

        return data;
    }

    // Private helper method to load data from the API
    public async Task<List<EnergyData>> LoadDataAsync(string date)
    {
        try
        {
            // Fetch data from the API for the given date
            var response = await _httpClient.GetAsync("https://www.elprisenligenu.dk/api/v1/prices/" + date + "_DK1.json");
            if (response.IsSuccessStatusCode)
            {
                // Deserialize the JSON response into a list of EnergyData
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<EnergyData>>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            else
            {
                // Return null if the response status code indicates an error
                return null;
            }
        }
        catch (Exception)
        {
            // Handle any exceptions that occur during the API call
            return null;
        }
    }

    // Private helper method to check if current time is after 1 PM
    private bool IsAfterOnePM()
    {
        // Get the current local time
        DateTime currentTime = DateTime.Now;

        // Return true if it's 1 PM or later
        return currentTime.Hour >= 13;
    }
}
