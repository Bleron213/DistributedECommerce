using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxCommerce.Warehouse.Application.Common.Infrastructure
{
    public interface IMessageSender
    {
        Task SendMessageAsync(object message, string exchangeName);
    }
}
