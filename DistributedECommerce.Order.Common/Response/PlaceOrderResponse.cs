using System;
using System.Collections.Generic;
using System.Text;

namespace DistributedECommerce.Orders.Common.Response
{
    public class PlaceOrderResponse
    {
        public Guid OrderId { get; set; }
        public string OrderNumber { get; set; }

    }
}
