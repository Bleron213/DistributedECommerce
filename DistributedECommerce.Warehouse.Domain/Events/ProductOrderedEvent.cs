using DistributedECommerce.Warehouse.Domain.Common;
using DistributedECommerce.Warehouse.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedECommerce.Warehouse.Domain.Events
{
    public class ProductOrderedEvent : BaseEvent
    {
        public ProductOrderedEvent(Product product)
        {
            Product = product;
        }

        public Product Product { get; }
    }    
}
