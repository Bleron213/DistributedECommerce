using System.Collections.Generic;

namespace DistributedECommerce.Warehouse.Common.Request
{
    public class MarkProductInOrderRequest
    {
        public List<string> ProductIds { get; set; } = new List<string>();
        public string OrderNumber { get; set; }
    }
}
