﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DistributedECommerce.Orders.Common.Messages
{
    public class OrderCreatedMessageRequest
    {
        public string OrderId { get; set; }
        public List<OrderProduct> Products { get; set; } = new List<OrderProduct>();
        public class OrderProduct
        {
            public string ProductCode { get; set; }
            public string? ProductId { get; set; }
            public string Components { get; set; }
        }
    }
}
