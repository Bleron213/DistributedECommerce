using BoxCommerce.Orders.Common.Requests;
using BoxCommerce.Orders.Common.Response;
using BoxCommerce.Warehouse.Application.Common.Infrastructure;

namespace BoxCommerce.Warehouse.Application.Orders.Commands
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
            private readonly IBoxCommerceOrderDbContext _boxCommerceOrderDbContext;

            public PlaceOrderCommandHandler(IBoxCommerceOrderDbContext boxCommerceOrderDbContext)
            {
                _boxCommerceOrderDbContext = boxCommerceOrderDbContext;
            }

            public async Task<PlaceOrderResponse> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
            {
                _boxCommerceOrderDbContext.Orders.Add(new Domain.Entities.Order("shaban", Guid.NewGuid()));
                await _boxCommerceOrderDbContext.SaveChangesAsync();

                return new PlaceOrderResponse();
            }
        }
    }
}
