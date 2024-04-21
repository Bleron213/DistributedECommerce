using DistributedECommerce.Orders.Application.Orders.Commands;
using DistributedECommerce.Utils.Errors;
using DistributedECommerce.Warehouse.Application.Stocks.Queries;
using DistributedECommerce.Warehouse.Common.Request;
using DistributedECommerce.Warehouse.Common.Response;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DistributedECommerce.Warehouse.API.Controllers.V1
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
        [ProducesResponseType(200, Type = typeof(InStockResponse))]
        [ProducesResponseType(400, Type = typeof(ErrorDetails))]
        public async Task<IActionResult> CheckInStock(InStockRequest request)
        {
            _logger.LogInformation("Entering method {method}", nameof(CheckInStock));

            var result = await _mediator.Send(new CheckInStockQuery(request));

            _logger.LogInformation("leaving method {method}", nameof(CheckInStock));

            return Ok(result);
        }
                
      
    }
}
