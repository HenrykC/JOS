using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ServiceBus
{
    public class TopicReciver
    {
        private readonly IModel _channel;

        public TopicReciver()
        {
            var protokoll = "amqp";
            var userName = ""; // todo, read from config and inject 
            var password = "";
            var hostName = "localhost";
            var port = 5672;

            var endpoints = new System.Collections.Generic.List<AmqpTcpEndpoint> {
                new AmqpTcpEndpoint("ServerName"),
                new AmqpTcpEndpoint("localhost")
            };

            ConnectionFactory factory = new ConnectionFactory();
            factory.UserName = userName;
            factory.Password = password;
            factory.VirtualHost = "/";
            //factory.pr.Protocol = Protocols.FromEnvironment();
            factory.HostName = hostName;
            factory.Port = AmqpTcpEndpoint.UseDefaultPort;

            var connection = factory.CreateConnection(endpoints);
            _channel = connection.CreateModel();
        }

        public void Register<T>(T type, EventHandler<BasicDeliverEventArgs> action)
        {
            var exchangeName = "Exchange";

            _channel.ExchangeDeclare(exchange: exchangeName, ExchangeType.Direct, durable: true);

           var queueName = _channel.QueueDeclare().QueueName;
           _channel.QueueBind(queue: queueName,
                exchange: exchangeName,
                routingKey: "");

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += action;
        }
    }
}
