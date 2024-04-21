using DistributedECommerce.Utils.Exceptions;
using DistributedECommerce.Warehouse.Domain.Enums;
using DistributedECommerce.Warehouse.Domain.Errors.Stock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DistributedECommerce.Warehouse.Domain.Entities
{
    public class Component : BaseEntity
    {
        public Component(string code, Guid? productId)
        {
            Id = Guid.NewGuid();
            Code = code;
            ProductId = productId;
            Status = ComponentStatus.SCHEDULED;
        }

        public Component(string code )
        {
            Id = Guid.NewGuid();
            Code = code;
            Status = ComponentStatus.SCHEDULED;
        }

        public string Code { get; private set; }
        public string? SerialCode { get; private set; }
        public ComponentStatus Status { get; private set; }
        public Guid? ProductId { get; private set; }
        #region Navigation
        public Product Product { get; set; }
        #endregion

        public void SetProductId(Guid productId)
        {
            ProductId = productId;
        }

        public void OrderCanceled()
        {
            switch (Status)
            {
                case ComponentStatus.SCHEDULED:
                    Status = ComponentStatus.CANCELLED;
                    break;
                case ComponentStatus.READY:
                    ProductId = null;
                    break;
            }
            
        }
    }
}
