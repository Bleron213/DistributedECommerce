using BoxCommerce.Orders.Application.Common.Infrastructure;
using BoxCommerce.Orders.Common.Requests;
using BoxCommerce.Orders.Common.Response;
using BoxCommerce.Orders.Domain.Events;
using BoxCommerce.Warehouse.ApiClient.Abstractions;
using Newtonsoft.Json;
using System.Text.Json;

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
            private readonly IOrderDbContext _orderDbContext;
            private readonly IWarehouseApiClient _warehouseApiClient;
            private readonly ICurrentUserService _currentUserService;

            public PlaceOrderCommandHandler(
                IOrderDbContext orderDbContext,
                IWarehouseApiClient warehouseApiClient,
                ICurrentUserService currentUserService
                )
            {
                _orderDbContext = orderDbContext;
                _warehouseApiClient = warehouseApiClient;
                _currentUserService = currentUserService;
            }

            // ToDo: Validate. Types must be distinct
            public async Task<PlaceOrderResponse> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
            {
                var stockResponse = await _warehouseApiClient.Stock.CheckInStock(new Warehouse.Common.Request.InStockRequest
                {
                    ProductCode = request.Payload.VehicleCode,
                    CustomComponents = request.Payload.Components.Select(x => new Warehouse.Common.Request.InStockRequest.ComponentStockRequest
                    {
                        Code = x.Code,
                    }).ToList()
                });

                // This is a case of a distributed transaction.
                if(stockResponse.Data.Status == Warehouse.Common.Response.InStockResponse.StockStatus.IN_STOCK)
                {
                    var order = new Domain.Entities.Order(Guid.Parse(_currentUserService.UserId));
                    order.AddOrderProduct(new Domain.Entities.OrderProduct(stockResponse.Data.ProductId, stockResponse.Data.Code, request.Payload.Components.Select(x => x.Code).ToList()));
                    order.AddDomainEvent(new OrderCreatedEvent(order));
                    await _orderDbContext.Orders.AddAsync(order);
                    // Would be a good idea to have a try/catch block that regenerates the ordernumber if it already exists. 
                    await _orderDbContext.SaveChangesAsync();

                    return new PlaceOrderResponse
                    {
                        OrderId = order.Id,
                        OrderNumber = order.OrderNumber
                    };
                } else
                {
                    var order = new Domain.Entities.Order(Guid.Parse(_currentUserService.UserId));
                    order.AddOrderProduct(new Domain.Entities.OrderProduct(stockResponse.Data.Code, request.Payload.Components.Select(x => x.Code).ToList()));
                    order.AddDomainEvent(new OrderCreatedEvent(order));
                    await _orderDbContext.Orders.AddAsync(order);

                    // Would be a good idea to have a try/catch block that regenerates the ordernumber if it already exists. 
                    await _orderDbContext.SaveChangesAsync();
                    return new PlaceOrderResponse
                    {
                        OrderId = order.Id,
                        OrderNumber = order.OrderNumber
                    };
                }
            }
        }

    }
}
