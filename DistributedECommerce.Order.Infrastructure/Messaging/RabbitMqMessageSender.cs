using DistributedECommerce.Orders.Application.Common.Infrastructure;
using DistributedECommerce.Orders.Application.Configurations;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace DistributedECommerce.Orders.Infrastructure.Messaging
{
    public class RabbitMqMessageSender : IMessageSender
    {
        private IConnection _connection;

        private const string OrderCreated_Queue = "order-created";

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
            channel.QueueDeclare(OrderCreated_Queue, false, false, false, null);

            channel.QueueBind(OrderCreated_Queue, exchangeName, "order-created");

            var json = JsonConvert.SerializeObject(message);
            var body = Encoding.UTF8.GetBytes(json);

            channel.BasicPublish(exchange: exchangeName, "order-created", null, body: body);
        }
    }
}
