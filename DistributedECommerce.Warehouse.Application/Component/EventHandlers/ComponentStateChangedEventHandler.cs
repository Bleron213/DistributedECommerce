using DistributedECommerce.Warehouse.Application.Common.Infrastructure;
using DistributedECommerce.Warehouse.Domain.Events;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedECommerce.Warehouse.Application.Component.EventHandlers
{
    public class ComponentStateChangedEventHandler : INotificationHandler<ComponentStateChangedEvent>
    {
        private readonly ILogger<ComponentStateChangedEventHandler> _logger;
        private readonly IMessageSender _messageSender;

        public ComponentStateChangedEventHandler(
            ILogger<ComponentStateChangedEventHandler> logger,
            IMessageSender messageSender
            )
        {
            _logger = logger;
            _messageSender = messageSender;
        }

        public async Task Handle(ComponentStateChangedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Component State Changed Event Handler {DomainEvent}", notification.GetType().Name);

            throw new NotImplementedException();
        }
    }
}
