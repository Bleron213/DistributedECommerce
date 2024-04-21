using DistributedECommerce.Utils.Errors;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace DistributedECommerce.Warehouse.Domain.Errors.Stock
{
    public static class WarehouseErrors
    {
        public static CustomError NegativeStockNumber => new CustomError(HttpStatusCode.BadRequest, "stock_number_negative", "Stock must never be less than zero");

        public static CustomError InvalidStatusMove(string OldStatus, string NewStatus) => new CustomError(HttpStatusCode.BadRequest, "invalid_status_move", $"Cannot move from Status {OldStatus} to {NewStatus}");
    }
}
