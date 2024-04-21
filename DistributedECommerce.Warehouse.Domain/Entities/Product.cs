using DistributedECommerce.Utils.Exceptions;
using DistributedECommerce.Warehouse.Domain.Enums;

namespace DistributedECommerce.Warehouse.Domain.Entities
{
    public class Product : BaseEntity
    {
        public Product()
        {
        }

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

        public void OrderCanceled()
        {
            OrderNumber = null;
            switch (Status)
            {
                case ProductStatus.IN_PROCESS:
                    Status = ProductStatus.CANCELLED;
                    break;
            }

            foreach (var item in Components)
            {
                item.OrderCanceled();
            }
        }


        #endregion

    }
}
