using BoxCommerce.Warehouse.Application.Common.Infrastructure;
using BoxCommerce.Warehouse.Common.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BoxCommerce.Orders.Application.Orders.Commands
{
    public class MarkProductInOrderCommand : IRequest
    {
        public MarkProductInOrderCommand(MarkProductInOrderRequest payload)
        {
            Payload = payload;
        }

        public MarkProductInOrderRequest Payload { get; }

        public class MarkProductInOrderCommandHandler : IRequestHandler<MarkProductInOrderCommand>
        {
            private readonly IWarehouseDbContext _warehouseDbContext;

            public MarkProductInOrderCommandHandler(
                IWarehouseDbContext warehouseDbContext
                )
            {
                _warehouseDbContext = warehouseDbContext;
            }

            public async Task Handle(MarkProductInOrderCommand request, CancellationToken cancellationToken)
            {
                var productIdGuids = request.Payload.ProductIds.Select(Guid.Parse).ToList();
                await _warehouseDbContext.Products
                    .Where(x => productIdGuids.Contains(x.Id))
                    .ExecuteUpdateAsync(setters => setters.SetProperty(b => b.OrderNumber, request.Payload.OrderNumber));
            }
        }
    }
}
