using BoxCommerce.Warehouse.Application.Common.Infrastructure;
using BoxCommerce.Warehouse.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxCommerce.Warehouse.Application.Product.EventHandlers
{
    public class ProductOrderedEventHandler : INotificationHandler<ProductOrderedEvent>
    {
        private readonly IMessageSender _messageSender;

        public ProductOrderedEventHandler(
            IMessageSender messageSender
            )
        {
            _messageSender = messageSender;
        }

        public Task Handle(ProductOrderedEvent notification, CancellationToken cancellationToken)
        {


            throw new NotImplementedException();
        }
    }
}
