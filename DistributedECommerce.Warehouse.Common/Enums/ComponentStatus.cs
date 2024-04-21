using System;
using System.Collections.Generic;
using System.Text;

namespace DistributedECommerce.Warehouse.Common.Enums
{
    public enum ComponentStatus
    {
        SCHEDULED = 0,
        IN_PROCESS = 1,
        READY_TO_ASSEMBLE = 2,
        ASSEMBLED = 3,
        CANCELLED = 4,
    }
}
