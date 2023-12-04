using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
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
    
    [NotMapped] // This attribute ensures the field is not included in the database
    public WallChargerState ChargerState { get; set; } = new OffState();
    
    private readonly HttpClient httpClient;
    
    public WallCharger(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public void TurnOn()
    {
        ChargerState.TurnOn(this, TurnOnUri);
        
        if (ChargerState is OffState)
        { 
            // TURN ON
        }
    }
    
    public void TurnOff()
    {
        ChargerState.TurnOff(this, TurnOffUri);
        
        if (ChargerState is OnState)
        { 
            // TURN OFF
        }
    }
    private async Task TurnOnCharger(string uri)
    {
        try
        {
            var response = await httpClient.PostAsync(uri, new StringContent(""));
            // response.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
    
    private async Task TurnOffCharger(string uri)
    {
        try
        {
            var response = await httpClient.PostAsync(uri, new StringContent(""));
            // response.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}