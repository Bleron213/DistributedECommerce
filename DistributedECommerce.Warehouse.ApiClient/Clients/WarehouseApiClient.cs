using BoxCommerce.Warehouse.ApiClient.Abstractions;
using BoxCommerce.Warehouse.ApiClient.Abstractions.Requests;

namespace BoxCommerce.Warehouse.ApiClient.Clients
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
