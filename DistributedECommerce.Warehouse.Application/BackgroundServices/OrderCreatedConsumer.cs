using DistributedECommerce.Orders.Common.Messages;
using DistributedECommerce.Warehouse.Application.Common.Infrastructure;
using DistributedECommerce.Warehouse.Application.Configurations;
using DistributedECommerce.Warehouse.Domain.Entities;
using DistributedECommerce.Warehouse.Domain.Enums;
using DistributedECommerce.Warehouse.Domain.Events;
using Dapper;
using Microsoft.AspNetCore.Connections;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using DistributedECommerce.Warehouse.Common.Message;

namespace DistributedECommerce.Warehouse.Application.BackgroundServices
{
    public class OrderCreatedConsumer : BackgroundService, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IServiceProvider _services;
        private readonly IConfiguration _configuration;


        private IWarehouseDbContext _warehouseDbContext;
        private IDbConnection _db;
        private ILogger<OrderCreatedConsumer> _logger;

        public OrderCreatedConsumer(
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
            _channel.QueueDeclare("order-created", false, false, false, null);
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                var orderCreatedMessage = JsonConvert.DeserializeObject<OrderCreatedMessageRequest>(content);
                await HandleOrderCreated(orderCreatedMessage);
                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume("order-created", false, consumer);
        }

        private async Task HandleOrderCreated(OrderCreatedMessageRequest orderCreatedMessage)
        {
            using var scope = _services.CreateScope();
            _logger = scope.ServiceProvider.GetService<ILogger<OrderCreatedConsumer>>()!;
            _warehouseDbContext = scope.ServiceProvider.GetRequiredService<IWarehouseDbContext>();



            try
            {
                // Would be a good idea to mark this entire block of code into a transaction scope.
                // If any step fails, all fail, and order also fails as a consequence
                // Although it would need some refactor. Perhaps using the same scope instead of dividing it in two parts
                await MarkExistingProductsInOrder(orderCreatedMessage);
                await OrderNewProducts(orderCreatedMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception while processing Order Create");
                // Mark Order as failed
                throw;
            }
            finally
            {
                // Event to acknowledge that the order was placed and that the order can move to the next phase
            }
        }

        private async Task OrderNewProducts(OrderCreatedMessageRequest orderCreatedMessage)
        {
            var newProducts = orderCreatedMessage.Products.Where(x => string.IsNullOrEmpty(x.ProductId)).ToList();

            if (newProducts.Count == 0)
                return;

            using var scope = _services.CreateScope();
            var dbConnectionString = _configuration["ConnectionStrings:DatabaseConnection"];
            _db = new SqlConnection(dbConnectionString);
            _warehouseDbContext = scope.ServiceProvider.GetRequiredService<IWarehouseDbContext>();

            foreach (var newProduct in newProducts)
            {
                var product = new Domain.Entities.Product(newProduct.ProductCode, orderCreatedMessage.OrderId);
                var components = JsonConvert.DeserializeObject<List<string>>(newProduct.Components);
                if (components != null && components.Count != 0)
                {
                    await CheckComponents(product, components);
                }

                product.AddDomainEvent(new ProductOrderedEvent(product));
                _warehouseDbContext.Products.Add(product);
            }
            await _warehouseDbContext.SaveChangesAsync();
        }

        private async Task CheckComponents(Domain.Entities.Product product, List<string>? components)
        {
            var valuesClause = string.Join(",", components.Select((opt, index) => $"('{opt}')"));

            var componentStatuses = await _db.QueryAsync<ComponentStockStatus>($@"
with cteValues as (
	SELECT *
	FROM (VALUES
		{valuesClause}
	) AS temp_table(componentCode)
) 
select 
	c.Id,
	cteValues.componentCode, 
	CASE
		WHEN c.Id is null THEN 'NOT_IN_STOCK'
		ELSE 'IN_STOCK'
	END as ComponentStatus
from cteValues
left join Components c on c.Code = cteValues.componentCode and c.Status = @Status and c.ProductId is null
", new { Status = ComponentStatus.READY_TO_ASSEMBLE });

            foreach (var component in componentStatuses)
            {
                if (component.ComponentStatus == "NOT_IN_STOCK")
                {
                    product.Components.Add(new Domain.Entities.Component(component.ComponentCode));
                }
            }

            // Would be a good idea to introduce concurrency handles, in case another instance of this consumer already marks 
            // these components as part of another product while savechanges has not finished
            var inStockComponents = componentStatuses.Where(x => x.ComponentStatus == "IN_STOCK").ToList();
            if (inStockComponents.Count != 0)
            {
                var inStockComponentsDb = await _warehouseDbContext.Components.Where(x => inStockComponents.Select(x => x.Id).Contains(x.Id)).ToListAsync();
                foreach (var item in inStockComponentsDb)
                {
                    item.SetProductId(product.Id);
                }
            }
        }

        private async Task MarkExistingProductsInOrder(OrderCreatedMessageRequest orderCreatedMessage)
        {
            var existingProducts = orderCreatedMessage.Products.Where(x => !string.IsNullOrEmpty(x.ProductId)).Select(x => x.ProductId!).ToList();

            if (existingProducts.Count == 0)
                return;

            using var scope = _services.CreateScope();
            var dbConnectionString = _configuration["ConnectionStrings:DatabaseConnection"];
            using IDbConnection db = new SqlConnection(dbConnectionString);

            // Would be a good idea to place them in a READ COMMITED transaction
            await db.ExecuteAsync(@"
                        UPDATE dbo.Products 
                        Set OrderNumber = @OrderNumber,
                            Status = @Status
                        WHERE Id in @ProductIds
                        ", new { OrderNumber = orderCreatedMessage.OrderId, ProductIds = existingProducts, Status = (int)ProductStatus.ASSEMBLED });
        }

        public override void Dispose()
        {
            _db.Dispose();
            base.Dispose();
        }

        public class ComponentStockStatus
        {
            public Guid? Id { get; set; }
            public string ComponentCode { get; set; }
            public string ComponentStatus { get; set; }
        }
    }


}
