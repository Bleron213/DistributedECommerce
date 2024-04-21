using DistributedECommerce.Orders.Common.Messages;
using DistributedECommerce.Warehouse.Application.Common.Infrastructure;
using DistributedECommerce.Warehouse.Application.Configurations;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Data;
using System.Text;

namespace DistributedECommerce.Warehouse.Application.BackgroundServices
{
    public class OrderCanceledConsumer : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IServiceProvider _services;
        private readonly IConfiguration _configuration;

        private IWarehouseDbContext _dbContext;
        private IDbConnection _db;


        public OrderCanceledConsumer(
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
            _channel.QueueDeclare("order-canceled", false, false, false, null);
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                var orderCreatedMessage = JsonConvert.DeserializeObject<OrderCanceledMessageRequest>(content);
                await HandleOrderCanceled(orderCreatedMessage);
                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume("order-canceled", false, consumer);
        }

        private async Task HandleOrderCanceled(OrderCanceledMessageRequest orderCanceledMessage)
        {
            using var scope = _services.CreateScope();
            _dbContext = scope.ServiceProvider.GetRequiredService<IWarehouseDbContext>();

            var products = await _dbContext.Products
                .Where(x => x.OrderNumber == orderCanceledMessage.OrderId)
                .Include(x => x.Components)
                .ToListAsync();

            foreach (var product in products)
            {
                product.OrderCanceled();
            }

            await _dbContext.SaveChangesAsync();
        }
    }


}
