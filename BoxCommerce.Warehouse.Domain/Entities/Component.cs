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
        public Component(string code, Guid? productId, string serialCode, ComponentType type)
        {
            Id = Guid.NewGuid();
            Code = code;
            Type = type;
            ProductId = productId;
            Status = ComponentStatus.SCHEDULED;
            SerialCode = serialCode;
        }

        public Component(string code, string serialCode, ComponentType type)
        {
            Id = Guid.NewGuid();
            Code = code;
            Type = type;
            Status = ComponentStatus.SCHEDULED;
            SerialCode = serialCode;
        }

        public string Code { get; private set; }
        public ComponentType Type { get; private set; }
        public string SerialCode { get; private set; }
        public ComponentStatus Status { get; private set; }
        public Guid? ProductId { get; private set; }
        #region Navigation
        public Product Product { get; set; }
        #endregion
    }
}
