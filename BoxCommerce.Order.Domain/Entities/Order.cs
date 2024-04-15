using BoxCommerce.Orders.Domain.Entities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxCommerce.Orders.Domain.Entities
{

    public class Order : BaseEntity, IAuditable
    {
        public Order
            (
            string orderNumber,
            Guid customerId
            )
        {
            Id = Guid.NewGuid();
            OrderNumber = orderNumber;
            CustomerId = customerId;
            Status = OrderStatus.IN_PROCESS;
        }

        public string OrderNumber { get; private set; }
        public OrderStatus Status { get; private set; }
        public string? Reason { get; private set; }
        public Guid CustomerId { get; private set; }

        public void CancelOrder(string reason)
        {
            Reason = reason;
            Status = OrderStatus.CANCELLED;
        }

        public void DeliverOrder()
        {
            Status = OrderStatus.DELIVERED;
        }
    }

    public enum OrderStatus
    {
        IN_PROCESS,
        DELIVERED,
        CANCELLED
    }
}
