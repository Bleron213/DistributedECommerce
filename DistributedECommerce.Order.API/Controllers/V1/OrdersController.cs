using DistributedECommerce.Orders.Application.Common.Infrastructure;
using DistributedECommerce.Orders.Application.Orders.Commands;
using DistributedECommerce.Orders.Application.Orders.Queries;
using DistributedECommerce.Orders.Common.Requests;
using DistributedECommerce.Orders.Common.Response;
using DistributedECommerce.Utils.Errors;
using DistributedECommerce.Warehouse.ApiClient.Abstractions;
using DistributedECommerce.Warehouse.ApiClient.Clients;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DistributedECommerce.Orders.API.Controllers.V1
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

        [HttpPost("CancelOrder/{orderId:Guid}")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400, Type = typeof(ErrorDetails))]
        public async Task<IActionResult> CancelOrder(Guid orderId)
        {
            _logger.LogInformation("Entering method {method}", nameof(CancelOrder));

             await _mediator.Send(new CancelOrderCommand(orderId));

            _logger.LogInformation("leaving method {method}", nameof(CancelOrder));

            return Accepted();
        }

        [HttpGet("OrderStatus/{orderNumber}")]
        [ProducesResponseType(200, Type = typeof(OrderStatusResponse))]
        [ProducesResponseType(400, Type = typeof(ErrorDetails))]
        public async Task<IActionResult> GetOrderStatus(string orderNumber)
        {
            _logger.LogInformation("Entering method {method}", nameof(CancelOrder));

            var result = await _mediator.Send(new GetOrderStatusQuery(orderNumber));

            _logger.LogInformation("leaving method {method}", nameof(CancelOrder));

            return Ok(result);
        }

    }
}
