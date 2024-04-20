using BoxCommerce.Warehouse.Domain.Enums;

namespace BoxCommerce.Warehouse.Domain.Entities
{
    public class Product : BaseEntity
    {
        public Product(string code, string orderNumber)
        {
            Id = Guid.NewGuid();
            Code = code;
            OrderNumber = orderNumber;
            Status = ProductStatus.IN_PROCESS;
        }        
        
        public string Code { get; private set; }
        public string? SerialCode { get; private set; }
        public ProductStatus Status { get; private set; }
        public string? OrderNumber { get; private set; }

        #region Navigation
        public List<Component> Components { get; private set; } = new();
        #endregion

    }
}
