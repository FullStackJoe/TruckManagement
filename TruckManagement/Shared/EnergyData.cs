using System.Text.Json.Serialization;

namespace TruckManagement.Shared;

public class EnergyData
{
    public double DKK_per_kWh { get; set; }
    // public double EUR_per_kWh { get; set; }
    // public double EXR { get; set; }
    
    [JsonPropertyName("time_start")]
    public DateTimeOffset TimeStart { get; set; }

    [JsonPropertyName("time_end")]
    public DateTimeOffset TimeEnd { get; set; }
}
