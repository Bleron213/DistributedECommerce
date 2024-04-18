using BoxCommerce.Orders.Domain.Events;
using BoxCommerce.Warehouse.ApiClient.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxCommerce.Orders.Application.Orders.EventHandlers
{
    public class OrderCreatedReorderProductEventHandler : INotificationHandler<OrderCreatedReorderProductEvent>
    {
        private readonly ILogger<OrderCreatedReorderProductEventHandler> _logger;
        private readonly IWarehouseApiClient _warehouseApiClient;

        public OrderCreatedReorderProductEventHandler(
            ILogger<OrderCreatedReorderProductEventHandler> logger,
            IWarehouseApiClient warehouseApiClient
            )
        {
            _logger = logger;
            _warehouseApiClient = warehouseApiClient;
        }

        public async Task Handle(OrderCreatedReorderProductEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Order Created Event Handler {DomainEvent}", notification.GetType().Name);

            await _warehouseApiClient.Stock.MarkProductInOrder(new Warehouse.Common.Request.MarkProductInOrderRequest
            {
                ProductIds = notification.Order.OrderedProducts.Select(x => x.ProductId).ToList(),
                OrderNumber = notification.Order.OrderNumber
            });
        }
    }
}
