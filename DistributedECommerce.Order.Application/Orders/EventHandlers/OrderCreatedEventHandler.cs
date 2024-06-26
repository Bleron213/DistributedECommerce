﻿using DistributedECommerce.Orders.Application.Common.Infrastructure;
using DistributedECommerce.Orders.Common.Messages;
using DistributedECommerce.Orders.Domain.Errors.Order;
using DistributedECommerce.Orders.Domain.Events;
using DistributedECommerce.Utils.Exceptions;
using DistributedECommerce.Warehouse.ApiClient.Abstractions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DistributedECommerce.Orders.Common.Messages.OrderCreatedMessageRequest;

namespace DistributedECommerce.Orders.Application.Orders.EventHandlers
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
                OrderId = notification.Order.Id.ToString(),
                Products = notification.Order.OrderedProducts.Select(x => new OrderProduct
                {
                    ProductCode = x.ProductCode,
                    ProductId = x.ProductId,
                    Components = x.ComponentCodes
                }).ToList()
            };
            await _messageSender.SendMessageAsync(message, "order:order-created");
        }
    }    
    
    public class OrderCanceledEventHandler : INotificationHandler<OrderCanceledEvent>
    {
        private readonly ILogger<OrderCanceledEventHandler> _logger;
        private readonly IMessageSender _messageSender;

        public OrderCanceledEventHandler(
            ILogger<OrderCanceledEventHandler> logger,
            IMessageSender messageSender
            )
        {
            _logger = logger;
            _messageSender = messageSender;
        }

        public async Task Handle(OrderCanceledEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Order Canceled Event Handler {DomainEvent}", notification.GetType().Name);
            var message = new OrderCanceledMessageRequest
            {
                OrderId = notification.Order.Id.ToString(),
            };
            await _messageSender.SendMessageAsync(message, "order:order-canceled");
        }
    }
}
