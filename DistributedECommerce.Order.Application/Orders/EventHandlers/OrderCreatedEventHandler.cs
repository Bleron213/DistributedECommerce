using BoxCommerce.Orders.Application.Common.Infrastructure;
using BoxCommerce.Orders.Common.Messages;
using BoxCommerce.Orders.Domain.Errors.Order;
using BoxCommerce.Orders.Domain.Events;
using BoxCommerce.Utils.Exceptions;
using BoxCommerce.Warehouse.ApiClient.Abstractions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BoxCommerce.Orders.Common.Messages.OrderCreatedMessageRequest;

namespace BoxCommerce.Orders.Application.Orders.EventHandlers
{
    public class OrderCreatedEventHandler : INotificationHandler<OrderCreatedEvent>
    {
        private readonly ILogger<OrderCreatedEventHandler> _logger;
        private readonly IMessageSender _messageSender;

        public OrderCreatedEventHandler(
            ILogger<OrderCreatedEventHandler> logger,
            IMessageSender messageSender
            )
        {
            _logger = logger;
            _messageSender = messageSender;
        }

        public async Task Handle(OrderCreatedEvent notification, CancellationToken cancellationToken)
        {
            if (notification.Order.OrderedProducts == null || notification.Order.OrderedProducts.Count == 0)
                throw new AppException(OrderErrors.NullOrEmptyProductList);

            _logger.LogInformation("Order Created Event Handler {DomainEvent}", notification.GetType().Name);
            var message = new OrderCreatedMessageRequest
            {
                OrderNumber = notification.Order.Id.ToString(),
                Products = notification.Order.OrderedProducts.Select(x => new OrderProduct
                {
                    ProductCode = x.ProductCode,
                    ProductId = x.ProductId,
                    Components = x.ComponentCodes
                }).ToList()
            };
            await _messageSender.SendMessageAsync(message, "product:ordercreate");
        }
    }
}
