using WebApplication1.Shared.ModelInterfaces;

namespace WebApplication1.Shared;

public class OnState : IWallChargerState
{
    private static readonly bool isCharging = true;
    
    public void TurnOn(WallCharger charger) {
        
    }

    public void TurnOff(WallCharger charger)
    {
        charger.ChargerState = new OffState();
    }
    

}