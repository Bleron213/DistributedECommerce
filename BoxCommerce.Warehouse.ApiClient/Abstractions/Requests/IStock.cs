using BoxCommerce.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BoxCommerce.Warehouse.ApiClient.Abstractions.Requests
{
    public interface IStock
    {
        Task<ResponseResult> SendRequest(bool throwOnException = true);
    }
}
