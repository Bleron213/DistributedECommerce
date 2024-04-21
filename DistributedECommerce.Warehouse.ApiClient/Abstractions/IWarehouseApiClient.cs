using BoxCommerce.Warehouse.ApiClient.Abstractions.Requests;

namespace BoxCommerce.Warehouse.ApiClient.Abstractions
{
    public interface IWarehouseApiClient
    {
        IStock Stock { get; }

    }
}
