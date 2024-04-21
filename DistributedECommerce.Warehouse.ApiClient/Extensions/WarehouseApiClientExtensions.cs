using BoxCommerce.Warehouse.ApiClient.Abstractions;
using BoxCommerce.Warehouse.ApiClient.Abstractions.Requests;
using BoxCommerce.Warehouse.ApiClient.Clients;
using BoxCommerce.Warehouse.ApiClient.Clients.Requests;
using BoxCommerce.Warehouse.ApiClient.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http.Headers;

namespace BoxCommerce.Warehouse.ApiClient.Extensions
{
    public static class WarehouseApiClientExtensions
    {
        public static void AddWarehouseApiClient(this IServiceCollection services, WarehouseApiClientConfiguration configuration)
        {
            services.AddHttpClient("WarehouseApiClient", client =>
            {
                var serviceProvider = services.BuildServiceProvider();
                var httpContextAccessor = serviceProvider.GetService<IHttpContextAccessor>();


                client.BaseAddress = new Uri(configuration.BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var correlationIdFound = httpContextAccessor.HttpContext.Request.Headers.TryGetValue("x-correlation-id", out var correlationId);
                if (correlationIdFound)
                {
                    client.DefaultRequestHeaders.Add("x-correlation-id", correlationId.ToString()); // Github requires a user-agent
                }

            });

            services.AddScoped<IWarehouseApiClient, WarehouseApiClient>();
            services.AddScoped<IStock, Stock>();

            
        }
    }
}
