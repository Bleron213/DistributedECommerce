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
    public class ProductUpdateConsumer : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IServiceProvider _services;
        private readonly IConfiguration _configuration;

        private IOrderDbContext _dbContext;
        private IDbConnection _db;
        private ILogger<ProductUpdateConsumer> _logger;

        public ProductUpdateConsumer(
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
            _channel.QueueDeclare("product-state-change", false, false, false, null);
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                var productStateChangedMessage = JsonConvert.DeserializeObject<ProductStateChangedMessage>(content);
                await HandleProductStateChange(productStateChangedMessage);
                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume("product-state-change", false, consumer);
        }

        private async Task HandleProductStateChange(ProductStateChangedMessage productStateChangedMessage)
        {
            using var scope = _services.CreateScope();
            _dbContext = scope.ServiceProvider.GetRequiredService<IOrderDbContext>();
            _logger = scope.ServiceProvider.GetRequiredService<ILogger<ProductUpdateConsumer>>();

            var orderedProduct = await _dbContext.OrderProducts.FirstOrDefaultAsync(x => x.Id == productStateChangedMessage.ProductId);
            if(orderedProduct == null)
            {
                _logger.LogWarning("No product was found with id = {id}", productStateChangedMessage.ProductId);
                return;
            }

            orderedProduct.StateChange(productStateChangedMessage.Status);
            var order = await _dbContext.Orders.Include(x => x.OrderedProducts).FirstAsync(x => x.Id == orderedProduct.Id);
            order.ProductStateChanged();
        }
    }
}
