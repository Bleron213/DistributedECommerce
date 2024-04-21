using System;
using System.Collections.Generic;
using System.Text;

namespace DistributedECommerce.Warehouse.Common.Enums
{
    public enum  ProductStatus
    {
        IN_PROCESS = 0,
        READY_TO_ASSEMBLE = 1,
        ASSEMBLED = 2,
        DELIVERED = 3,
        CANCELLED = 4
    }
}
