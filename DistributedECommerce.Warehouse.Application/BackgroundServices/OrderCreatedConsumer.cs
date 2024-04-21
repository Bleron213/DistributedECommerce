using BoxCommerce.Orders.Common.Messages;
using BoxCommerce.Warehouse.Application.Common.Infrastructure;
using BoxCommerce.Warehouse.Application.Configurations;
using BoxCommerce.Warehouse.Domain.Entities;
using BoxCommerce.Warehouse.Domain.Enums;
using BoxCommerce.Warehouse.Domain.Events;
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

namespace BoxCommerce.Warehouse.Application.BackgroundServices
{
    public class OrderCreatedConsumer : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IServiceProvider _services;
        private readonly IConfiguration _configuration;

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
            var logger = scope.ServiceProvider.GetService<ILogger<OrderCreatedConsumer>>()!;
            var dbContext = scope.ServiceProvider.GetRequiredService<IWarehouseDbContext>();



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
                logger.LogError(ex, "Exception while processing Order Create");
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
            using IDbConnection db = new SqlConnection(dbConnectionString);
            var dbContext = scope.ServiceProvider.GetRequiredService<IWarehouseDbContext>();

            foreach (var newProduct in newProducts)
            {
                var product = new Domain.Entities.Product(newProduct.ProductCode, orderCreatedMessage.OrderNumber);
                var components = JsonConvert.DeserializeObject<List<string>>(newProduct.Components);
                if (components != null && components.Count != 0)
                {
                    await CheckComponents(db, dbContext, product, components);
                }

                product.AddDomainEvent(new ProductOrderedEvent(product));
                dbContext.Products.Add(product);
            }
            await dbContext.SaveChangesAsync();
        }

        private async Task CheckComponents(IDbConnection db, IWarehouseDbContext dbContext, Domain.Entities.Product product, List<string>? components)
        {
            var valuesClause = string.Join(",", components.Select((opt, index) => $"('{opt}')"));

            var componentStatuses = await db.QueryAsync<ComponentStockStatus>($@"
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
", new { Status = ComponentStatus.READY });

            foreach (var component in componentStatuses)
            {
                if (component.ComponentStatus == "NOT_IN_STOCK")
                {
                    product.Components.Add(new Component(component.ComponentCode));
                }
            }

            // Would be a good idea to introduce concurrency handles, in case another instance of this consumer already marks 
            // these components as part of another product while savechanges has not finished
            var inStockComponents = componentStatuses.Where(x => x.ComponentStatus == "IN_STOCK").ToList();
            if (inStockComponents.Count != 0)
            {
                var inStockComponentsDb = await dbContext.Components.Where(x => inStockComponents.Select(x => x.Id).Contains(x.Id)).ToListAsync();
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
                        ", new { OrderNumber = orderCreatedMessage.OrderNumber, ProductIds = existingProducts, Status = (int)ProductStatus.ASSEMBLED });
        }

        public class ComponentStockStatus
        {
            public Guid? Id { get; set; }
            public string ComponentCode { get; set; }
            public string ComponentStatus { get; set; }
        }
    }
}
