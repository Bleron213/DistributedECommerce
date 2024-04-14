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

        public OrdersController(ISender mediator)
        {
            _mediator = mediator;
        }


        [HttpPost("Order/Place")]
        public async Task<IActionResult> PlaceOrder(PlaceOrderRequest placeOrderRequest)
        {            
            var result = await _mediator.Send(new PlaceOrderCommand(placeOrderRequest));
            return Ok(result);
        }

        
    }
}
