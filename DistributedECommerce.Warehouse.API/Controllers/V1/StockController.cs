using BoxCommerce.Orders.Application.Orders.Commands;
using BoxCommerce.Utils.Errors;
using BoxCommerce.Warehouse.Application.Stocks.Queries;
using BoxCommerce.Warehouse.Common.Request;
using BoxCommerce.Warehouse.Common.Response;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
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
        [ProducesResponseType(200, Type = typeof(InStockResponse))]
        [ProducesResponseType(400, Type = typeof(ErrorDetails))]
        public async Task<IActionResult> CheckInStock(InStockRequest request)
        {
            _logger.LogInformation("Entering method {method}", nameof(CheckInStock));

            var result = await _mediator.Send(new CheckInStockQuery(request));

            _logger.LogInformation("leaving method {method}", nameof(CheckInStock));

            return Ok(result);
        }
                
        
        [HttpPost("MarkProductInOrder")]
        [ProducesResponseType(202)]
        [ProducesResponseType(400, Type = typeof(ErrorDetails))]
        public async Task<IActionResult> MarkProductInOrder(MarkProductInOrderRequest request)
        {
            _logger.LogInformation("Entering method {method}", nameof(MarkProductInOrder));

            await _mediator.Send(new MarkProductInOrderCommand(request));

            _logger.LogInformation("leaving method {method}", nameof(MarkProductInOrder));

            return Accepted();
        }


    }
}
