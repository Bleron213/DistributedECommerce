using DistributedECommerce.Warehouse.Common.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DistributedECommerce.Warehouse.Common.Message
{
    public class ProductStateChangedMessage
    {
        public Guid ProductId { get; set; }
        public string? OrderId { get; set; }
        public ProductStatus Status { get; set; }
    }
}
