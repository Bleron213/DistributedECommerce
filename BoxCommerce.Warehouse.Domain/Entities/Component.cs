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
        public Component(string code, string serialCode, ComponentType type)
        {
            Id = Guid.NewGuid();
            Code = code;
            Type = type;
            ComponentStatus = ComponentStatus.SCHEDULED;
            SerialCode = serialCode;
        }

        public string Code { get; private set; }
        public ComponentType Type { get; private set; }
        public string SerialCode { get; private set; }
        public ComponentStatus ComponentStatus { get; private set; }
    }    

    public class Stock : BaseEntity
    {
        public Stock(string code, int quantity)
        {
            Code = code;
            Quantity = quantity; 
        }

        public string Code { get; set; }
        public int Quantity { get; set; }

        public void UpdateQuantity(int quantity)
        {
            if (quantity < 0)
                throw new AppException(StockErrors.NegativeStockNumber);

            Quantity = quantity;
        }

        public void IncreaseStock()
        {
            Quantity++;
        }

        public void DecreaseStock()
        {
            Quantity++;

            if (Quantity < 0)
                Quantity = 0;
        }
    }
}
