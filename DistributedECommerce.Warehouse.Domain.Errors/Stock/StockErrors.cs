using BoxCommerce.Utils.Errors;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace BoxCommerce.Warehouse.Domain.Errors.Stock
{
    public static class StockErrors
    {
        public static CustomError NegativeStockNumber => new CustomError(HttpStatusCode.BadRequest, "stock_number_negative", "Stock must never be less than zero");
    }
}
