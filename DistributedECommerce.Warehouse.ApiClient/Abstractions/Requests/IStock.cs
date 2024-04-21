using BoxCommerce.Utils;
using BoxCommerce.Warehouse.Common.Request;
using BoxCommerce.Warehouse.Common.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BoxCommerce.Warehouse.ApiClient.Abstractions.Requests
{
    public interface IStock
    {
        Task<ResponseResult<InStockResponse>> CheckInStock(InStockRequest request, bool throwOnException = true);
        Task<ResponseResult> MarkProductInOrder(MarkProductInOrderRequest request, bool throwOnException = true);
    }
}
