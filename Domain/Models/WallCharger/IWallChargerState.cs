namespace WebApplication1.Shared.ModelInterfaces;

public interface IWallChargerState
{
    public void TurnOn(WallCharger charger);
    public void TurnOff(WallCharger charger);
}