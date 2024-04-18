using BoxCommerce.Orders.Domain.Common;
using BoxCommerce.Orders.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxCommerce.Orders.Domain.Events
{
    public class OrderCreatedReorderProductEvent : BaseEvent
    {
        public OrderCreatedReorderProductEvent(Order order)
        {
            Order = order;
        }

        public Order Order { get; }
    }


}
