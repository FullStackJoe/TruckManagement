using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Text.Json.Serialization;
using WebApplication1.Shared.ModelInterfaces;

namespace WebApplication1.Shared;

public class WallCharger
{
    [Key]
    public int ChargerId { get; set; }
    
    [Required]
    public int ChargerAmpere { get; set; }
    
    [Required]
    public string TurnOffUri { get; set; }
    
    [Required]
    public string TurnOnUri { get; set; }
    
    [JsonIgnore]
    [NotMapped] // This attribute ensures the field is not included in the database
    public IWallChargerState ChargerState { get; set; } = new OffState();
    
    private HttpClient httpClient;
    
    public WallCharger()
    {
    }
    public WallCharger(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }
    public void SetHttpClient(HttpClient httpClient)
    {
        this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public void TurnOn()
    {
        if (ChargerState is OffState)
        {
            ApiCallOn();
        }
        ChargerState.TurnOn(this);
    }
    
    public void TurnOff()
    {
        if (ChargerState is OnState)
        {
            ApiCallOff();
        }
        ChargerState.TurnOff(this);
    }
    private async Task ApiCallOn()
    {
        try
        {
            var response = await httpClient.PostAsync(TurnOnUri, new StringContent(""));
            response.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    
    private async Task ApiCallOff()
    {
        try
        {
            var response = await httpClient.PostAsync(TurnOffUri, new StringContent(""));
            response.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}