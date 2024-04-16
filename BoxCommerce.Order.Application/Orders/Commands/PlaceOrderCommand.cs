using BoxCommerce.Orders.Application.Common.Infrastructure;
using BoxCommerce.Orders.Common.Requests;
using BoxCommerce.Orders.Common.Response;
using BoxCommerce.Warehouse.ApiClient.Abstractions;

namespace BoxCommerce.Orders.Application.Orders.Commands
{
    public class PlaceOrderCommand : IRequest<PlaceOrderResponse>
    {
        public PlaceOrderRequest Payload { get; }
        public PlaceOrderCommand(PlaceOrderRequest payload)
        {
            ArgumentNullException.ThrowIfNull(payload);
            Payload = payload;

        }

        public class PlaceOrderCommandHandler : IRequestHandler<PlaceOrderCommand, PlaceOrderResponse>
        {
            private readonly IOrderDbContext _boxCommerceOrderDbContext;
            private readonly IWarehouseApiClient _warehouseApiClient;

            public PlaceOrderCommandHandler(
                IOrderDbContext boxCommerceOrderDbContext,
                IWarehouseApiClient warehouseApiClient
                )
            {
                _boxCommerceOrderDbContext = boxCommerceOrderDbContext;
                _warehouseApiClient = warehouseApiClient;
            }

            public async Task<PlaceOrderResponse> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
            {
                // Check if vehicle exists
                //var result1 = await _warehouseApiClient.Stock.CheckInStock();


                return new PlaceOrderResponse();
            }
        }
    }
}
