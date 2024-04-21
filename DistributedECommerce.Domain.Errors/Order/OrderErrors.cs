using DistributedECommerce.Utils.Errors;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace DistributedECommerce.Orders.Domain.Errors.Order
{
    public class OrderErrors
    {
        public static CustomError NullOrEmptyProductList => new CustomError(HttpStatusCode.InternalServerError, "null_or_empty_product_list", "Null or empty product list when creating a new order");
        public static CustomError InvalidStatusMove(string currentStatus, string newStatus) => new CustomError(HttpStatusCode.BadRequest, "invalid_status_move", $"Invalid status move. Cannot move from {currentStatus} to {newStatus}");

    }
}
