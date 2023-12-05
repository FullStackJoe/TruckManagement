using WebApplication1.Shared;

namespace HttpClients.ClientInterface;

public interface ITruckTypeService
{
    Task<TruckType> CreateAsync(TruckType newTruckType);
}