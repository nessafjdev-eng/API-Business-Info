using BusinessInfo.Application.Services.Interfaces;
using BusinessInfo.Common;
using Microsoft.AspNetCore.Connections;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BusinessInfo.Application.Services
{
    public class RabbitMqService : IMessageQueueService
    {
        private readonly ConnectionFactory _factory;

        public RabbitMqService()
        {
            _factory = new ConnectionFactory
            {
                HostName = Configuration.RabbitHost,
                Port = Configuration.RabbitPort,
                UserName = Configuration.RabbitUsername,
                Password = Configuration.RabbitPassword
            };
        }

        public void Publish<T>(T data, string exchange, string routingKey)
        {
            using var connection = _factory.CreateConnection("business-producer");
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare(
                exchange: exchange,
                type: ExchangeType.Direct,
                durable: true
            );

            var json = JsonSerializer.Serialize(data);
            var body = Encoding.UTF8.GetBytes(json);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(
                exchange: exchange,
                routingKey: routingKey,
                basicProperties: properties,
                body: body
            );
        }
        public void Consume<T>(
            string queue,
            string routingKey,
            string exchange,
            Action<T> onMessage
        )
        {
            var connection = _factory.CreateConnection("business-producer");
            var channel = connection.CreateModel();

            var deadLetterExchange = $"{exchange}.dlx";
            var deadLetterQueue = $"{queue}.dlq";
            var deadLetterRoutingKey = $"{routingKey}.dead";

            channel.ExchangeDeclare(exchange, ExchangeType.Direct, durable: true);
            channel.ExchangeDeclare(deadLetterExchange, ExchangeType.Direct, durable: true);

            var args = new Dictionary<string, object>
            {
                { "x-dead-letter-exchange", deadLetterExchange },
                { "x-dead-letter-routing-key", deadLetterRoutingKey }
            };

            channel.QueueDeclare(queue, true, false, false, args);
            channel.QueueBind(queue, exchange, routingKey);

            channel.QueueDeclare(deadLetterQueue, true, false, false);
            channel.QueueBind(deadLetterQueue, deadLetterExchange, deadLetterRoutingKey);

            channel.BasicQos(0, 1, false);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                try
                {
                    var json = Encoding.UTF8.GetString(ea.Body.ToArray());
                    var message = JsonSerializer.Deserialize<T>(json);

                    if (message == null)
                        throw new Exception("Mensagem inválida");

                    onMessage(message);

                    channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao processar mensagem: {ex.Message}");

                    channel.BasicNack(ea.DeliveryTag, false, requeue: false);
                }
            };

            channel.BasicConsume(queue, false, consumer);
        }
    }
}
