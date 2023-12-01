using WebApplication1.Shared;

namespace HttpClients.ClientInterface;

public interface IChargerService
{
    Task<WallCharger> CreateAsync(WallCharger newCharger);
}