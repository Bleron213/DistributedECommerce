using DistributedECommerce.Warehouse.Application.Common.Infrastructure;
using DistributedECommerce.Warehouse.Application.Configurations;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace DistributedECommerce.Warehouse.Infrastructure.Messaging
{
    public class RabbitMqMessageSender : IMessageSender
    {
        private IConnection _connection;

        private const string OrderUpdate_Queue = "order-state-change";
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RabbitMqMessageSender(
            RabbitMqConfiguration configuration,
            IHttpContextAccessor httpContextAccessor
            )
        {
            var factory = new ConnectionFactory
            {
                HostName = configuration.HostName,
                Password = configuration.Password,
                UserName = configuration.Username
            };

            _connection = factory.CreateConnection();
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task SendMessageAsync(object message, string exchangeName)
        {
            using var channel = _connection.CreateModel();
            channel.ExchangeDeclare(exchangeName, ExchangeType.Direct, durable: false);

            var queueInfo = GetQueueName(exchangeName);

            channel.QueueDeclare(queueInfo.QueueName, false, false, false, null);
            channel.QueueBind(queueInfo.QueueName, exchangeName, queueInfo.RoutingKey);

            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);

            var properties = channel.CreateBasicProperties();
            var correlationIdFound = _httpContextAccessor?.HttpContext?.Request?.Headers?.TryGetValue("x-correlation-id", out _);
            if (correlationIdFound != null && correlationIdFound.Value)
            {
                var correlation = _httpContextAccessor.HttpContext.Request.Headers["x-correlation-id"];
                properties.Headers = new Dictionary<string, object>();
                properties.Headers["x-correlation-id"] = correlation.ToString();
            }


            channel.BasicPublish(exchange: exchangeName, queueInfo.RoutingKey, null, body: body);
        }

        public (string QueueName, string RoutingKey) GetQueueName(string exchangeName)
        {
            switch (exchangeName)
            {
                case "order:order-state-change": return new(OrderUpdate_Queue, "order-state-change");
                default: throw new Exception("No queue found");
            }
        }
    }
}
