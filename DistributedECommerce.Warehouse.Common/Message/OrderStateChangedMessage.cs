using DistributedECommerce.Warehouse.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DistributedECommerce.Warehouse.Common.Message
{
    public class OrderStateChangedMessage
    {
        public string OrderId { get; set; }
    }
}
