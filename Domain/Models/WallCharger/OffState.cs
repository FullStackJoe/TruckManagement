using WebApplication1.Shared.ModelInterfaces;
namespace WebApplication1.Shared;

public class OffState : IWallChargerState
{
    private static readonly bool isCharging = false;
    
    public void TurnOn(WallCharger charger) {
        charger.ChargerState = new OnState();
    }

    public void TurnOff(WallCharger charger)
    {
    }
    
    
}