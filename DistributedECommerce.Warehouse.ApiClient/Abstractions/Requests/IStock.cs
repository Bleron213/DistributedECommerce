using DistributedECommerce.Utils;
using DistributedECommerce.Warehouse.Common.Request;
using DistributedECommerce.Warehouse.Common.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DistributedECommerce.Warehouse.ApiClient.Abstractions.Requests
{
    public interface IStock
    {
        Task<ResponseResult<InStockResponse>> CheckInStock(InStockRequest request, bool throwOnException = true);
        Task<ResponseResult> MarkProductInOrder(MarkProductInOrderRequest request, bool throwOnException = true);
    }
}
