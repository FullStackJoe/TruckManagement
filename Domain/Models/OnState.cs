using WebApplication1.Shared.ModelInterfaces;

namespace WebApplication1.Shared;

public class OnState : WallChargerState
{
    private static readonly bool isCharging = true;
    
    public void TurnOn(WallCharger charger, string uri) {
        
    }

    public void TurnOff(WallCharger charger, string uri)
    {
        charger.ChargerState = new OffState();
    }
    

}