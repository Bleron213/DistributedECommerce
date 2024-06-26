﻿using DistributedECommerce.Utils;
using DistributedECommerce.Utils.Errors.CoreErrors;
using DistributedECommerce.Utils.Errors;
using DistributedECommerce.Utils.Exceptions;
using DistributedECommerce.Warehouse.ApiClient.Abstractions.Requests;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DistributedECommerce.Warehouse.Common.Request;
using DistributedECommerce.Warehouse.Common.Response;

namespace DistributedECommerce.Warehouse.ApiClient.Clients.Requests
{
    public class Stock : IStock
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public Stock(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<ResponseResult<InStockResponse>> CheckInStock(InStockRequest request, bool throwOnException = true)
        {
            var client = _httpClientFactory.CreateClient("WarehouseApiClient");

            string jsonString = JsonSerializer.Serialize(request);
            var stringContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"api/v1/Stock/CheckInStock", stringContent);

            var content = await response.Content.ReadAsStringAsync();

            var responseResult = new ResponseResult<InStockResponse>(response.IsSuccessStatusCode, response.StatusCode, content);
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

        public async Task<ResponseResult> MarkProductInOrder(MarkProductInOrderRequest request, bool throwOnException = true)
        {
            var client = _httpClientFactory.CreateClient("WarehouseApiClient");

            string jsonString = JsonSerializer.Serialize(request);
            var stringContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"api/v1/Stock/MarkProductInOrder", stringContent);

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
