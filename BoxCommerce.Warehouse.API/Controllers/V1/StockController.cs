using BoxCommerce.Orders.Application.Common.Infrastructure;
using BoxCommerce.Orders.Application.Orders.Commands;
using BoxCommerce.Orders.Common.Requests;
using BoxCommerce.Warehouse.API.Controllers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoxCommerce.Warehouse.API.Controllers.V1
{
    public class StockController : ApiBaseController
    {
        private readonly ISender _mediator;
        private readonly ILogger<StockController> _logger;

        public StockController(
            ISender mediator,
            ILogger<StockController> logger
            )
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("ShabanDashi")]
        public async Task<IActionResult> sHABAN()
        {
            _logger.LogInformation("In Shaban method");
            return Ok();
        }


    }
}
