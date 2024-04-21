using DistributedECommerce.Orders.Application.Orders.Commands;
using DistributedECommerce.Utils.Errors;
using DistributedECommerce.Warehouse.Application.Stocks.Queries;
using DistributedECommerce.Warehouse.Common.Enums;
using DistributedECommerce.Warehouse.Common.Request;
using DistributedECommerce.Warehouse.Common.Response;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Configuration;
using System.Text;

namespace DistributedECommerce.Warehouse.API.Controllers.V1
{
    public class TestController : ApiBaseController
    {
        private readonly ILogger<TestController> _logger;
        private readonly IConfiguration _configuration;

        public TestController(
            ILogger<TestController> logger,
            IConfiguration configuration
            )
        {
            _logger = logger;
            _configuration = configuration;
        }


        [HttpPost("ComponentCompleted/{componentId}")]
        [ProducesResponseType(200, Type = typeof(InStockResponse))]
        [ProducesResponseType(400, Type = typeof(ErrorDetails))]
        public async Task<IActionResult> ComponentCompleted(Guid componentId, ComponentStatus oldStatus, ComponentStatus newStatus)
        {
            var factory = new ConnectionFactory
            {
                HostName = _configuration["RabbitMqConfiguration:HostName"],
                Password = _configuration["RabbitMqConfiguration:Password"],
                UserName = _configuration["RabbitMqConfiguration:Username"]
            };

            var _connection = factory.CreateConnection();

            using var channel = _connection.CreateModel();

            var exchangeName = "component:component-completed";
            var queueName = "component-completed";
            var routingKey = "component-completed";
            channel.ExchangeDeclare(exchangeName, ExchangeType.Direct, durable: false);

            channel.QueueDeclare(queueName, false, false, false, null);
            channel.QueueBind(queueName, exchangeName, routingKey);

            var msg = new ComponentStatusChange
            {
                ComponentId = componentId,
                OldStatus = oldStatus,
                NewStatus = newStatus
            };

            var json = JsonConvert.SerializeObject(msg);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: exchangeName, routingKey, null, body: body);

            return Ok();
        }
               


    }

    public class ComponentStatusChange
    {
        public Guid ComponentId { get; set; }
        public ComponentStatus OldStatus { get; set; }
        public ComponentStatus NewStatus { get; set; }
    }
}
