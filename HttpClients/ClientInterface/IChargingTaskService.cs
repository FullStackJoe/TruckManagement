using WebApplication1.Shared;

namespace HttpClients.ClientInterface;

public interface IChargingTaskService
{
    void CreateAsync(ChargingTask newCharger);
}