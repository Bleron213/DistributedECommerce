using System;
using System.Collections.Generic;
using System.Text;

namespace BoxCommerce.Orders.Common.Response
{
    public class ProductResponse
    {
        public ProductResponse(string displayName, string name)
        {
            DisplayName = displayName;
            Name = name;
        }

        public string DisplayName { get; set; }
        public string Name { get; set; }
    }
}
