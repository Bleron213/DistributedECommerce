using DistributedECommerce.Warehouse.Domain.Common;
using DistributedECommerce.Warehouse.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedECommerce.Warehouse.Domain.Events
{
    public class ComponentStateChangedEvent : BaseEvent
    {
        public ComponentStateChangedEvent(Component component)
        {
            Component = component;
        }

        public Component Component { get; }
    }
}
