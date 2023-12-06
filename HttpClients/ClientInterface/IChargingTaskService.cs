using WebApplication1.Shared;

namespace HttpClients.ClientInterface;

public interface IChargingTaskService
{
    Task CreateAsync(ChargingTask newCharger);
}