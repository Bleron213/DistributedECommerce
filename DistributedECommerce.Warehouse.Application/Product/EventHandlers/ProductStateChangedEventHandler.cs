using DistributedECommerce.Warehouse.Application.Common.Infrastructure;
using DistributedECommerce.Warehouse.Common.Message;
using DistributedECommerce.Warehouse.Domain.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedECommerce.Warehouse.Application.Product.EventHandlers
{
    public class ProductStateChangedEventHandler : INotificationHandler<ProductStateChangedEvent>
    {
        private readonly ILogger<ProductStateChangedEventHandler> _logger;
        private readonly IMessageSender _sender;

        public ProductStateChangedEventHandler(
            ILogger<ProductStateChangedEventHandler> logger,
            IMessageSender sender
            )
        {
            _logger = logger;
            _sender = sender;
        }

        public async Task Handle(ProductStateChangedEvent notification, CancellationToken cancellationToken)
        {
            var message = new ProductStateChangedMessage
            {
                ProductId = notification.Product.Id,
                OrderId = notification.Product.OrderNumber,
                Status = (Warehouse.Common.Enums.ProductStatus)notification.Product.Status
            };
            await _sender.SendMessageAsync(message, "product:product-state-change");
        }
    }
}
