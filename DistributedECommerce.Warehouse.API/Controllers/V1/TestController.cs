using Dapper;
using DistributedECommerce.Orders.Application.Orders.Commands;
using DistributedECommerce.Utils.Errors;
using DistributedECommerce.Warehouse.Application.Common.Infrastructure;
using DistributedECommerce.Warehouse.Application.Stocks.Queries;
using DistributedECommerce.Warehouse.Common.Enums;
using DistributedECommerce.Warehouse.Common.Request;
using DistributedECommerce.Warehouse.Common.Response;
using DistributedECommerce.Warehouse.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace DistributedECommerce.Warehouse.API.Controllers.V1
{
    public class TestController : ApiBaseController
    {
        private readonly ILogger<TestController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IWarehouseDbContext _warehouseDbContext;

        public TestController(
            ILogger<TestController> logger,
            IConfiguration configuration,
            IWarehouseDbContext warehouseDbContext
            )
        {
            _logger = logger;
            _configuration = configuration;
            _warehouseDbContext = warehouseDbContext;
        }


        [HttpPost("ComponentCompleted/{componentId}")]
        [ProducesResponseType(200, Type = typeof(InStockResponse))]
        [ProducesResponseType(400, Type = typeof(ErrorDetails))]
        public async Task<IActionResult> ComponentCompleted(Guid componentId, Common.Enums.ComponentStatus oldStatus, Common.Enums.ComponentStatus newStatus)
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

    public class InsertProductReq
    {
        public string VehicleCode { get; set; }
        public string? OrderId { get; set; }
        public Domain.Enums.ProductStatus Status { get; set; }
        public class ComponentReq
        {
            public string Code { get; set; }
            public Domain.Enums.ComponentStatus ComponentStatus { get; set; }
        }
    }

    public class ComponentStatusChange
    {
        public Guid ComponentId { get; set; }
        public Common.Enums.ComponentStatus OldStatus { get; set; }
        public Common.Enums.ComponentStatus NewStatus { get; set; }
    }
}
