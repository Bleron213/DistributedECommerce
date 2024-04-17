using BoxCommerce.Warehouse.Domain.Enums;

namespace BoxCommerce.Warehouse.Domain.Entities
{
    public class Product : BaseEntity
    {
        public Product(string code, string serialCode, string propertiesHash = null)
        {
            Id = Guid.NewGuid();
            Code = code;
            SerialCode = serialCode;
            Status = ProductStatus.IN_PROCESS;
            PropertiesHash = propertiesHash;
        }        
        
        public Product(string code, string serialCode)
        {
            Id = Guid.NewGuid();
            Code = code;
            SerialCode = serialCode;
            Status = ProductStatus.IN_PROCESS;
        }

        public string Code { get; private set; }
        public string SerialCode { get; private set; }
        public string? PropertiesHash { get; private set; }
        public ProductStatus Status { get; private set; }



    }
}
