using BoxCommerce.Utils;
using BoxCommerce.Utils.Errors.CoreErrors;
using BoxCommerce.Utils.Errors;
using BoxCommerce.Utils.Exceptions;
using BoxCommerce.Warehouse.ApiClient.Abstractions.Requests;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BoxCommerce.Warehouse.ApiClient.Clients.Requests
{
    public class Stock : IStock
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public Stock(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<ResponseResult> SendRequest(bool throwOnException = true)
        {
            var client = _httpClientFactory.CreateClient("WarehouseApiClient");
            // TODO: Add authentication
            var response = await client.GetAsync($"api/v1/Stock/ShabanDashi");

            var content = await response.Content.ReadAsStringAsync();

            var responseResult = new ResponseResult(response.IsSuccessStatusCode, response.StatusCode, content);
            if (!responseResult.IsSuccessStatusCode)
            {
                if (responseResult.ErrorDetails != null && throwOnException)
                {
                    throw new AppException(new CustomError(responseResult.StatusCode, responseResult.ErrorDetails.ErrorMessage, responseResult.ErrorDetails.ErrorExceptionMessage));
                }

                if (throwOnException)
                {
                    throw new AppException(GenericErrors.ThirdPartyFailure);
                }
            }
            return responseResult;
        }
    }
}
