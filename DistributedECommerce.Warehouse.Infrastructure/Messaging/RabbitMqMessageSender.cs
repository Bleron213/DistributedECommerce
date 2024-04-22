using DistributedECommerce.Warehouse.Application.Common.Infrastructure;
using DistributedECommerce.Warehouse.Application.Configurations;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace DistributedECommerce.Warehouse.Infrastructure.Messaging
{
    public class RabbitMqMessageSender : IMessageSender
    {
        private IConnection _connection;

        private const string OrderUpdate_Queue = "order-state-change";

        public RabbitMqMessageSender(RabbitMqConfiguration configuration)
        {
            var factory = new ConnectionFactory
            {
                HostName = configuration.HostName,
                Password = configuration.Password,
                UserName = configuration.Username
            };

            _connection = factory.CreateConnection();
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
