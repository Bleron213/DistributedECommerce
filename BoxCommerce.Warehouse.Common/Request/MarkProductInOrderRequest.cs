using System.Collections.Generic;

namespace BoxCommerce.Warehouse.Common.Request
{
    public class MarkProductInOrderRequest
    {
        public List<string> ProductIds { get; set; } = new List<string>();
        public string OrderNumber { get; set; }
    }
}
