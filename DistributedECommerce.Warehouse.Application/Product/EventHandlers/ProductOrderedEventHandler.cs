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
        public ProductOrderedEventHandler(
            )
        {
        }

        public async Task Handle(ProductOrderedEvent notification, CancellationToken cancellationToken)
        {
            // Here we could send a message to a queue that informs the consumers in factory microservices to start producing a new product
        }
    }
}
