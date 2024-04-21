using System;
using System.Collections.Generic;
using System.Text;

namespace DistributedECommerce.Warehouse.Common.Request
{
    public class InStockRequest
    {
        public string ProductCode { get; set; }
        public List<ComponentStockRequest> CustomComponents { get; set; } = new List<ComponentStockRequest>();

        public class ComponentStockRequest
        {
            public string Code { get; set; }
        }
    }
}
