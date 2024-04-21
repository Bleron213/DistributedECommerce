using DistributedECommerce.Orders.Domain.Enums;
using Newtonsoft.Json;

namespace DistributedECommerce.Orders.Domain.Entities
{
    public class OrderProduct : BaseEntity
    {

        public OrderProduct(string? productId, string productCode, List<string> components)
        {
            Id = Guid.NewGuid();
            Status = ProductStatus.IN_PROCESS; 
            ProductId = productId;
            ProductCode = productCode;
            ComponentCodes = JsonConvert.SerializeObject(components);
        }        
        
        public OrderProduct(string productCode, List<string> components)
        {
            Id = Guid.NewGuid();
            Status = ProductStatus.IN_PROCESS;
            ProductCode = productCode;
            ComponentCodes = JsonConvert.SerializeObject(components);
        }

        public OrderProduct()
        {
        }

        /// <summary>
        /// Product Id in the Warehouse. If purchasing an existig Product
        /// </summary>
        public string? ProductId { get;  private set; }

        /// <summary>
        /// Code that marks the type of product we're trying to buy. If ProductId is null, it means that a Product with this code will have to be ordered
        /// </summary>
        public string ProductCode { get; private set; }
        public string ComponentCodes { get; private set; }
        public ProductStatus Status { get; private set; }
        public Guid OrderId { get; set; }
        #region Navigation
        public Order Order { get; set; }

        public void StateChange(DistributedECommerce.Warehouse.Common.Enums.ProductStatus status)
        {
            switch(status)
            {
                case Warehouse.Common.Enums.ProductStatus.ASSEMBLED:
                    Status = ProductStatus.READY;
                    break;
                case Warehouse.Common.Enums.ProductStatus.CANCELLED:
                    Status = ProductStatus.CANCELLED;
                    break;
                case Warehouse.Common.Enums.ProductStatus.DELIVERED:
                    Status = ProductStatus.DELIVERED;
                    break;
                default:
                    throw new Exception("Invalid status change");

            }
        }

        #endregion
    }
}
