using System;
using System.Collections.Generic;
using System.Text;

namespace DistributedECommerce.Warehouse.Common.Response
{
    public class InStockResponse
    {
        public string ProductId { get; set; }
        public string Code { get; set; }
        public StockStatus Status { get; set; }

        public List<InStockComponent> Components = new List<InStockComponent>();
        public class InStockComponent
        {
            public string ComponentId { get; set; }
            public string Code { get; set; }
            public StockStatus Status { get; set; }
        }

        public enum StockStatus
        {
            NOT_IN_STOCK,
            IN_STOCK,
        }
    }





}
