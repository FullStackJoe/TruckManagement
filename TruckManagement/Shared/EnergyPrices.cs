using System.Text.Json;

namespace TruckManagement.Shared;

public class EnergyPrices
{
    private readonly HttpClient _httpClient;
    string currentDate = DateTime.Now.ToString("yyyy/MM-dd");
    string tomorrowDate = DateTime.Now.AddDays(1).ToString("yyyy/MM-dd");
    

    public EnergyPrices(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<EnergyData>> getPriceDataAsync()
    {
        // Load data for the current date
        List<EnergyData> data = await LoadDataAsync(currentDate);

        // If it's after 1 PM, load data for tomorrow's date and append it to 'data'
        if (IsAfterOnePM())
        {
            List<EnergyData> tomorrowData = await LoadDataAsync(tomorrowDate);
            
            // Remove instances where TimeEnd is before NOW
            data = data.Where(d => d.TimeEnd >= DateTimeOffset.Now).ToList();
            if (tomorrowData != null)
            {
                // Append tomorrow's data to today's data. This assumes both lists are not null.
                data.AddRange(tomorrowData);
            }
        }

        return data;
    }

    public async Task<List<EnergyData>> LoadDataAsync(string date)
    {
        try
        {
            var response = await _httpClient.GetAsync("https://www.elprisenligenu.dk/api/v1/prices/"+ date +"_DK1.json");
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<EnergyData>>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            else
            {
                // Handle the error...
                return null;
            }
        }
        catch (Exception)
        {
            // Handle exceptions
            return null;
        }
    }
    private bool IsAfterOnePM()
    {
        // Get the current local time
        DateTime currentTime = DateTime.Now;

        // Check if the hour is greater than or equal to 13 (1 PM)
        return currentTime.Hour >= 13;
    }
}