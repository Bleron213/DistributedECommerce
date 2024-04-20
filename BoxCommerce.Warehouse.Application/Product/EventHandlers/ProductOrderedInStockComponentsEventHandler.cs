using BoxCommerce.Warehouse.Application.Common.Infrastructure;
using BoxCommerce.Warehouse.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxCommerce.Warehouse.Application.Product.EventHandlers
{
    internal class ProductOrderedInStockComponentsEventHandler : INotificationHandler<ProductOrderedInStockComponentsEvent>
    {
        private readonly IWarehouseDbContext _warehouseDbContext;

        public ProductOrderedInStockComponentsEventHandler(
            IWarehouseDbContext warehouseDbContext
            )
        {
            _warehouseDbContext = warehouseDbContext;
        }

        public Task Handle(ProductOrderedInStockComponentsEvent notification, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
