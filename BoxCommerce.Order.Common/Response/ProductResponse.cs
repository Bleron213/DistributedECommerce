using System;
using System.Collections.Generic;
using System.Text;

namespace BoxCommerce.Orders.Common.Response
{
    public class ProductResponse
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public string Name { get; set; }
    }
}
