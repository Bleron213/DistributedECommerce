using DistributedECommerce.Orders.Application.Common.Infrastructure;
using DistributedECommerce.Orders.Common.Response;
using DistributedECommerce.Orders.Domain.Entities;
using DistributedECommerce.Utils.Errors.CoreErrors;
using DistributedECommerce.Utils.Exceptions;
using DistributedECommerce.Warehouse.Common.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedECommerce.Orders.Application.Orders.Queries
{
    public class GetOrderStatusQuery : IRequest<OrderStatusResponse>
    {
        public GetOrderStatusQuery(string orderNumber)
        {
            ArgumentException.ThrowIfNullOrEmpty(orderNumber);
            OrderNumber = orderNumber;
        }

        public string OrderNumber { get; }
    }

    public class GetOrderStatusQueryHandler : IRequestHandler<GetOrderStatusQuery, OrderStatusResponse>
    {
        private readonly IOrderDbContext _orderDbContext;
        private readonly ICurrentUserService _currentUserService;

        public GetOrderStatusQueryHandler(
            IOrderDbContext orderDbContext,
            ICurrentUserService currentUserService
            )
        {
            _orderDbContext = orderDbContext;
            _currentUserService = currentUserService;
        }

        public async Task<OrderStatusResponse> Handle(GetOrderStatusQuery request, CancellationToken cancellationToken)
        {
            var order = await _orderDbContext.Orders.FirstOrDefaultAsync(x => x.OrderNumber == request.OrderNumber && x.CustomerId == _currentUserService.UserGuid) ?? throw new AppException(GenericErrors.NotFound);

            var orderInfo = GetOrderInfo(order);

            return new OrderStatusResponse
            {
                OrderNumber = order.OrderNumber,
                Message = orderInfo.Message,
                Status = orderInfo.Status.ToString()
            };
        }

        public (string Message, DistributedECommerce.Orders.Common.Enums.OrderStatus Status) GetOrderInfo(Order order)
        {
            switch (order.Status)
            {
                case Domain.Enums.OrderStatus.IN_PROCESS:
                    return new("Your order is in Processing", DistributedECommerce.Orders.Common.Enums.OrderStatus.IN_PROCESS);
                case Domain.Enums.OrderStatus.READY:
                    return new("Your order is ready for Delivery", DistributedECommerce.Orders.Common.Enums.OrderStatus.READY);                
                case Domain.Enums.OrderStatus.CANCELLED:
                    return new($"Your order has been cancelled with reason: {order.Reason}", DistributedECommerce.Orders.Common.Enums.OrderStatus.CANCELLED);
                case Domain.Enums.OrderStatus.DELIVERED:
                    return new($"Your order has been been delivered. We hope you are happy with what you ordered!", DistributedECommerce.Orders.Common.Enums.OrderStatus.DELIVERED);
                default:
                    throw new Exception("Unknown status");
            }
        }
    }
}
