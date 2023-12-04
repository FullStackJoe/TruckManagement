namespace WebApplication1.Shared.ModelInterfaces;

public interface WallChargerState
{
    public void TurnOn(WallCharger charger, string uri);
    public void TurnOff(WallCharger charger, string uri);
}