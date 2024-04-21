using DistributedECommerce.Warehouse.ApiClient.Abstractions;
using DistributedECommerce.Warehouse.ApiClient.Abstractions.Requests;

namespace DistributedECommerce.Warehouse.ApiClient.Clients
{
    public class WarehouseApiClient : IWarehouseApiClient
    {
        public WarehouseApiClient(
            IStock stock
            )
        {
            Stock = stock;
        }

        public IStock Stock { get; }
    }
}
