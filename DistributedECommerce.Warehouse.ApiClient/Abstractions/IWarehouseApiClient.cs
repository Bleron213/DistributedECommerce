using DistributedECommerce.Warehouse.ApiClient.Abstractions.Requests;

namespace DistributedECommerce.Warehouse.ApiClient.Abstractions
{
    public interface IWarehouseApiClient
    {
        IStock Stock { get; }

    }
}
