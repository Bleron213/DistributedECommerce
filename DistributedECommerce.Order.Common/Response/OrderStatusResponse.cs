using DistributedECommerce.Orders.Common.Enums;
using System;

namespace DistributedECommerce.Orders.Common.Response
{
    public class OrderStatusResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string OrderNumber { get; set; }

    }
}
