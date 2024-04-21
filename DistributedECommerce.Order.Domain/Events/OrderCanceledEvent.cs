using DistributedECommerce.Orders.Domain.Common;
using DistributedECommerce.Orders.Domain.Entities;

namespace DistributedECommerce.Orders.Domain.Events
{
    public class OrderCanceledEvent : BaseEvent
    {
        public OrderCanceledEvent(Order order)
        {
            Order = order;
        }

        public Order Order { get; }
    }


}
