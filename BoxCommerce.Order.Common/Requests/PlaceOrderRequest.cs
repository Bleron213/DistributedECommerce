using System;
using System.Collections.Generic;
using System.Text;

namespace BoxCommerce.Orders.Common.Requests
{
    public class PlaceOrderRequest
    {
        public string VehicleCode { get; set; }
        public List<Component> Components { get; set; }

        public class Component
        {
            public string Code { get; set; }
        }
    }

    
}
