using Microsoft.Azure.Cosmos;

namespace Fintech.Library.DataAccess.Concrete.Repository;

public interface ICosmosRepository
{
    Task BeginCosmos(string ContainerName, string PartionKey);
    Task<ItemResponse<T>> AddItemsToContainerAsync<T>(T Model);
    Task<List<ItemResponse<T>>> AddItemsToContainerAsync<T>(List<T> List);
    Task<List<T>> QueryItemsAsync<T>(string expression);
    Task<ContainerResponse> DeleteContainerAsync(string ContainerName);
}
