using BoxCommerce.Orders.Application.Common.Infrastructure;
using BoxCommerce.Orders.Application.Orders.Commands;
using BoxCommerce.Orders.Application.Products;
using BoxCommerce.Orders.Common.Requests;
using BoxCommerce.Orders.Common.Response;
using BoxCommerce.Warehouse.ApiClient.Abstractions;
using BoxCommerce.Warehouse.ApiClient.Clients;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoxCommerce.Orders.API.Controllers.V1
{
    public class ProductController : ApiBaseController
    {
        private readonly ISender _mediator;
        private readonly IWarehouseApiClient _warehouseApiClient;
        private readonly ILogger<OrdersController> _logger;

        public ProductController(
            ISender mediator,
            IWarehouseApiClient warehouseApiClient,
            ILogger<OrdersController> logger
            )
        {
            _mediator = mediator;
            _warehouseApiClient = warehouseApiClient;
            _logger = logger;
        }


        /// <summary>
        /// Hardcoded because not relevant
        /// 
        /// Imagine these products come from another Microservice and are cached here. 
        /// Every hour the list is refreshed 
        /// </summary>
        /// <returns></returns>
        [HttpGet("AvailableProducts")]
        public async Task<IActionResult> GetAvailableProducts()
        {
            return Ok(new List<ProductResponse>
            {
                new ProductResponse("Volkswagen Golf GTI", "VOLKSWAGEN_GOLF_GTI"),
                new ProductResponse("BMW M3", "BMW_M3"),
                new ProductResponse("Audi RS3", "AUDI_RS3"),
                new ProductResponse("Mercedes-Benz AMG C63", "MERCEDES_BENZ_AMG_C63"),
                new ProductResponse("Porsche 911 Carrera", "PORSCHE_911_CARRERA"),
                new ProductResponse("Volkswagen Passat R-Line", "VOLKSWAGEN_PASSAT_R_LINE"),
                new ProductResponse("BMW M5", "BMW_M5"),
                new ProductResponse("Audi RS6 Avant", "AUDI_RS6_AVANT"),
                new ProductResponse("Mercedes-Benz AMG E63", "MERCEDES_BENZ_AMG_E63"),
                new ProductResponse("Porsche Cayenne Turbo S", "PORSCHE_CAYENNE_TURBO_S")
            });
        }
    }
}
