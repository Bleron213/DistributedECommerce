using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedECommerce.Orders.Domain.Enums
{
    public enum ProductStatus
    {
        IN_PROCESS = 0,
        READY = 1,
        DELIVERED = 2,
        CANCELLED = 3
    }
}
