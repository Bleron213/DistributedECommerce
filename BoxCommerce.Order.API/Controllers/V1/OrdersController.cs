using BoxCommerce.Orders.Application.Common.Infrastructure;
using BoxCommerce.Orders.Application.Orders.Commands;
using BoxCommerce.Orders.Common.Requests;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoxCommerce.Orders.API.Controllers.V1
{
    public class OrdersController : ApiBaseController
    {
        private readonly ISender _mediator;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(
            ISender mediator,
            ILogger<OrdersController> logger
            )
        {
            _mediator = mediator;
            _logger = logger;
        }


        [HttpPost("Order/Place")]
        public async Task<IActionResult> PlaceOrder(PlaceOrderRequest placeOrderRequest)
        {
            _logger.LogInformation("Tedua");
            var result = await _mediator.Send(new PlaceOrderCommand(placeOrderRequest));
            return Ok(result);
        }

        
    }
}
