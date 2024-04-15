using System;
using System.Collections.Generic;
using System.Text;

namespace BoxCommerce.Orders.Common.Response
{
    public class ComponentResponse
    {
        public Guid Id { get; set; }
        public string ComponentCode { get; set; }
        public ComponentType ComponentType { get; set; }
    }

    public enum ComponentType
    {
        NotDefined = 0,
        ENGINE = 1, 
        CHASSIS = 2, 
        OPTION_PACK = 3
    }
}
