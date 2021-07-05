using System;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace ServiceBus
{
    public class TopicSender
    {

        private readonly IModel _channel;

        public TopicSender()
        {
            var protokoll = "amqp";
            var userName = ""; // todo, read from config and inject 
            var password = "";
            var hostName = "";
            var port = 5672;

            var endpoints = new System.Collections.Generic.List<AmqpTcpEndpoint> {
                new AmqpTcpEndpoint("Servername"),
                new AmqpTcpEndpoint("localhost")
            };

            ConnectionFactory factory = new ConnectionFactory();
            factory.UserName = userName;
            factory.Password = password;
            factory.VirtualHost = "/";
            //factory.pr.Protocol = Protocols.FromEnvironment();
            factory.HostName = hostName;
            factory.Port = AmqpTcpEndpoint.UseDefaultPort;


            // to do: docker run ? 
            //docker run --rm -it --hostname my-rabbit -p 15672:15672 -p 5672:5672 rabbitmq:3-management
            var connection = factory.CreateConnection(endpoints);
            _channel = connection.CreateModel();
        }

        public void Publish<T>(T message)
        {
            var eventName = message.GetType().Name;
            var exchangeName = "Exchange";

            _channel.ExchangeDeclare(exchange: exchangeName, ExchangeType.Direct, durable: true);

            var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
           
            _channel.BasicPublish(exchange: exchangeName,
                routingKey: eventName,
                basicProperties: null,
                body: body);
        }
    }
}
