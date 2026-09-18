using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessInfo.Application.Services.Interfaces
{
    public interface IMessageQueueService
    {
        void Publish<T>(T data, string exchange, string routingKey);
        void Consume<T>(
            string queue,
            string routingKey,
            string exchange,
            Action<T> onMessage
        );
    }
}
