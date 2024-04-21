using BoxCommerce.Orders.Application.Common.Infrastructure;
using BoxCommerce.Orders.Application.Orders.Commands;
using BoxCommerce.Orders.Common.Requests;
using BoxCommerce.Orders.Common.Response;
using BoxCommerce.Utils.Errors;
using BoxCommerce.Warehouse.ApiClient.Abstractions;
using BoxCommerce.Warehouse.ApiClient.Clients;
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


        [HttpPost("PlaceOrder")]
        [ProducesResponseType(201, Type = typeof(PlaceOrderResponse))]
        [ProducesResponseType(400, Type = typeof(ErrorDetails))]
        public async Task<IActionResult> PlaceOrder(PlaceOrderRequest placeOrderRequest)
        {
            _logger.LogInformation("Entering method {method}", nameof(PlaceOrder));

            var result = await _mediator.Send(new PlaceOrderCommand(placeOrderRequest));
            
            _logger.LogInformation("leaving method {method}", nameof(PlaceOrder));
            
            return Ok(result);
        }

        
    }
}
