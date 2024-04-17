using System;
using System.Collections.Generic;
using System.Text;

namespace BoxCommerce.Warehouse.Common.Request
{
    public class VehicleInStockRequest
    {
        public string ProductCode { get; set; }
        public List<ComponentStockRequest> CustomComponents { get; set; } = new List<ComponentStockRequest>();

        public class ComponentStockRequest
        {
            public string Code { get; set; }
            public string Type { get; set; }
        }
    }
}
