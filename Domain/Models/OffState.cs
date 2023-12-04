using WebApplication1.Shared.ModelInterfaces;
namespace WebApplication1.Shared;

public class OffState : WallChargerState
{
    private static readonly bool isCharging = false;
    
    public void TurnOn(WallCharger charger, string uri) {
        charger.ChargerState = new OnState();
        
        
        
    }

    public void TurnOff(WallCharger charger, string uri)
    {
    }
    
    
}