using BoxCommerce.Warehouse.Application.Stocks.Queries;
using BoxCommerce.Warehouse.Common.Request;
using MediatR;
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


        [HttpPost("CheckInStock")]
        public async Task<IActionResult> CheckInStock(VehicleInStockRequest request)
        {
            _logger.LogInformation("Entering method {method}", nameof(CheckInStock));

            var result = await _mediator.Send(new CheckInStockQuery(request));

            _logger.LogInformation("leaving method {method}", nameof(CheckInStock));

            return Ok(result);
        }


    }
}
