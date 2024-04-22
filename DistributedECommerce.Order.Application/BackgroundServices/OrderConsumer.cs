using DistributedECommerce.Orders.Application.Common.Infrastructure;
using DistributedECommerce.Orders.Application.Configurations;
using DistributedECommerce.Orders.Domain.Enums;
using DistributedECommerce.Warehouse.Common.Message;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Data;
using System.Text;

namespace DistributedECommerce.Orders.Application.BackgroundServices
{
    public class OrderConsumer : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IServiceProvider _services;
        private readonly IConfiguration _configuration;

        private IOrderDbContext _dbContext;
        private IDbConnection _db;
        private ILogger<OrderConsumer> _logger;

        public OrderConsumer(
            IServiceProvider services,
            IConfiguration configuration,
            RabbitMqConfiguration rabbitMqConfiguration
            )
        {
            _services = services;
            _configuration = configuration;
            var factory = new ConnectionFactory
            {
                HostName = rabbitMqConfiguration.HostName,
                Password = rabbitMqConfiguration.Password,
                UserName = rabbitMqConfiguration.Username
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare("order-state-change", false, false, false, null);
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (ch, ea) =>
            {
                using var scope = _services.CreateScope();
                _dbContext = scope.ServiceProvider.GetRequiredService<IOrderDbContext>();
                _logger = scope.ServiceProvider.GetRequiredService<ILogger<OrderConsumer>>();
               
                var valuesDictionary = new Dictionary<string, object>();

                var correlationIdFound = ea.BasicProperties?.Headers?.ContainsKey("x-correlation-id");
                if (correlationIdFound != null && correlationIdFound.Value)
                {
                    var correlationIdObj = ea!.BasicProperties!.Headers["x-correlation-id"];
                    var correlationIdByteArr = (byte[])correlationIdObj;

                    var correlationId = Encoding.UTF8.GetString(correlationIdByteArr);
                    valuesDictionary.Add("x-correlation-id", correlationId!);
                }

                using (_logger.BeginScope(valuesDictionary))
                {
                    try
                    {
                        var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                        var productStateChangedMessage = JsonConvert.DeserializeObject<OrderStateChangedMessage>(content);
                        await OrderStateChanged(productStateChangedMessage);
                        _channel.BasicAck(ea.DeliveryTag, false);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error in OrderConsumer");
                    }
                }


            };

            _channel.BasicConsume("order-state-change", false, consumer);
        }

        private async Task OrderStateChanged(OrderStateChangedMessage orderStateChangedMessage)
        {
            var orderId = Guid.Parse(orderStateChangedMessage.OrderId);
            var order = await _dbContext.Orders.FirstOrDefaultAsync(x => x.Id == orderId);
            order?.OrderReady();
            await _dbContext.SaveChangesAsync();

        }
    }
}
