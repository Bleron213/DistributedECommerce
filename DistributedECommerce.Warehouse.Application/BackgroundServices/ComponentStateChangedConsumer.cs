using DistributedECommerce.Warehouse.Application.Common.Infrastructure;
using DistributedECommerce.Warehouse.Application.Configurations;
using DistributedECommerce.Warehouse.Common.Enums;
using DistributedECommerce.Warehouse.Common.Message;
using DistributedECommerce.Warehouse.Domain.Entities;
using DistributedECommerce.Warehouse.Domain.Events;
using MediatR;
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
    public class ComponentStateChangedConsumer : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IServiceProvider _services;
        private readonly IConfiguration _configuration;

        private IWarehouseDbContext _dbContext;
        private ILogger<ComponentStateChangedConsumer> _logger;
        private IDbConnection _db;


        public ComponentStateChangedConsumer(
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
            _channel.QueueDeclare("component-completed", false, false, false, null);
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                var orderCreatedMessage = JsonConvert.DeserializeObject<ComponentStatusChange>(content);
                await HandleComponentStateChanged(orderCreatedMessage);
                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume("component-completed", false, consumer);
        }

        private async Task HandleComponentStateChanged(ComponentStatusChange componentStatusChange)
        {
            using var scope = _services.CreateScope();
            _dbContext = scope.ServiceProvider.GetRequiredService<IWarehouseDbContext>();
            _logger = scope.ServiceProvider.GetRequiredService<ILogger<ComponentStateChangedConsumer>>();

            try
            {
                var component = await _dbContext.Components
                    .Include(x => x.Product)
                    .FirstOrDefaultAsync(x => x.Id == componentStatusChange.ComponentId) ?? throw new Exception($"Could not find component with Id = {componentStatusChange.ComponentId}");

                component.ComponentStateChange((Domain.Enums.ComponentStatus)componentStatusChange.NewStatus);

                await _dbContext.SaveChangesAsync();

                if (component.ProductId is not null)
                {
                    var product = await _dbContext.Products.Include(x => x.Components).Where(x => x.Id == component.ProductId!).FirstAsync();
                    product.ProductStatusUpdateCheck();
                    await _dbContext.SaveChangesAsync();
                }


                if (component.ProductId is not null && component.Product.OrderNumber is not null)
                {
                    var productsReady = await _dbContext.Products.Where(x => x.OrderNumber == component.Product.OrderNumber).AllAsync(x => x.Status == Domain.Enums.ProductStatus.ASSEMBLED);
                    if (productsReady)
                    {
                        var msgSender = scope.ServiceProvider.GetService<IMessageSender>();
                        var message = new OrderStateChangedMessage
                        {
                            OrderId = component.Product.OrderNumber,
                        };
                        await msgSender.SendMessageAsync(message, "order:order-state-change");
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ComponentStateChange Consumer");
            }

        }

        public class ComponentStatusChange
        {
            public Guid ComponentId { get; set; }
            public ComponentStatus NewStatus { get; set; }
        }
    }
}
