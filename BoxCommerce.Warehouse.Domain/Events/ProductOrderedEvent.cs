using BoxCommerce.Warehouse.Domain.Common;
using BoxCommerce.Warehouse.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxCommerce.Warehouse.Domain.Events
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
