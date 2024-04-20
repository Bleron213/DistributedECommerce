using BoxCommerce.Utils.Exceptions;
using BoxCommerce.Warehouse.Domain.Enums;
using BoxCommerce.Warehouse.Domain.Errors.Stock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BoxCommerce.Warehouse.Domain.Entities
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
    }
}
