using BoxCommerce.Warehouse.Domain.Common;
using BoxCommerce.Warehouse.Domain.Entities;

namespace BoxCommerce.Warehouse.Domain.Events
{
    public class ProductOrderedInStockComponentsEvent : BaseEvent
    {
        public ProductOrderedInStockComponentsEvent(Product product, List<ComponentStockStatus> inStockComponents)
        {
            Product = product;
        }

        public Product Product { get; }

        public class ComponentStockStatus
        {
            public Guid Id { get; set; }
            public string ComponentCode { get; set; }
            public string ComponentStatus { get; set; }
        }
    }
}
