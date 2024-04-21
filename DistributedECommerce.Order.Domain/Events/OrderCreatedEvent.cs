using DistributedECommerce.Orders.Domain.Common;
using DistributedECommerce.Orders.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedECommerce.Orders.Domain.Events
{
    public class OrderCreatedEvent : BaseEvent
    {
        public OrderCreatedEvent(Order order)
        {
            Order = order;
        }

        public Order Order { get; }
    }


}
