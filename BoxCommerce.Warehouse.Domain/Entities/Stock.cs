using BoxCommerce.Utils.Exceptions;
using BoxCommerce.Warehouse.Domain.Enums;
using BoxCommerce.Warehouse.Domain.Errors.Stock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxCommerce.Warehouse.Domain.Entities
{
    public class Stock : BaseEntity
    {
        public Stock(string code, StockType type, int stockNumber = 0)
        {
            Id = Guid.NewGuid();
            Code = code;
            Type = type;
            StockNumber = stockNumber;
        }

        public string Code { get; private set; }
        public StockType Type { get; private set; }
        public int StockNumber { get; private set; }

        public void UpdateStock(int number)
        {
            if (number < 0)
                throw new AppException(StockErrors.NegativeStockNumber);

            StockNumber = number;
        }
    }
}
