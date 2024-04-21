using DistributedECommerce.Orders.Domain.Entities.Interfaces;
using DistributedECommerce.Utils.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DistributedECommerce.Orders.Domain.Errors.Order;
using DistributedECommerce.Orders.Domain.Enums;

namespace DistributedECommerce.Orders.Domain.Entities
{

    public class Order : BaseEntity, IAuditable
    {
        public Order
            (
            Guid customerId
            ) 
        {
            Id = Guid.NewGuid();
            OrderNumber = GenerateOrderNumber();
            CustomerId = customerId;
            Status = OrderStatus.IN_PROCESS;
        }

        public Order()
        {
        }

        public string OrderNumber { get; private set; }
        public OrderStatus Status { get; private set; }
        public string? Reason { get; private set; }
        public Guid CustomerId { get; private set; }

        #region Navigation
        public ICollection<OrderProduct> OrderedProducts { get; private set; } = [];
        #endregion

        public void AddOrderProduct(OrderProduct orderProduct)
        {
            OrderedProducts.Add(orderProduct);
        }

        public void OrderReady()
        {
            switch (Status)
            {
                case OrderStatus.IN_PROCESS:
                    Status = OrderStatus.READY;
                    break;
                default:
                    throw new AppException(OrderErrors.InvalidStatusMove(Status.ToString(), OrderStatus.READY.ToString()));
            }
        }

        public void CancelOrder(string reason)
        {
            Reason = reason;
            Status = OrderStatus.CANCELLED;
        }

        public void DeliverOrder()
        {
            Status = OrderStatus.DELIVERED;
        }

        internal string GenerateOrderNumber()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            const int orderNumberLength = 8;

            char[] orderNumber = new char[orderNumberLength];

            for (int i = 0; i < orderNumberLength; i++)
            {
                orderNumber[i] = chars[random.Next(chars.Length)];
            }

           return new string(orderNumber);
        }

        public void ProductStateChanged()
        {
            var productsReady = OrderedProducts.All(x => x.Status == ProductStatus.READY);
            if (productsReady)
            {
                OrderReady();
            }

            var productsCancelled = OrderedProducts.All(x => x.Status == ProductStatus.CANCELLED);
            if (productsCancelled)
            {
                CancelOrder("Cancelled by Third Party");
            }
        }
    }
}
