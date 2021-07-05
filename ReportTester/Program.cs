using System;
using System.Diagnostics;
using System.Threading;
using RabbitMQ.Client.Events;
using ServiceBus;
using ServiceBus.Commands.Outlook;

namespace ReportTester
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread.Sleep(5000);
            var sender = new TopicSender();
            var reciver = new TopicReciver();

            reciver.Register(typeof(CreateAvailabilities), Action);

            var createAvailabilities = new CreateAvailabilities()
            {
                UserName = "Cwikowski@HiScout.com",
                StartDate = new DateTime(2021, 06, 21),
                EndDate = new DateTime(2021, 06, 25),
            };


            while (true)
            {
                sender.Publish(createAvailabilities);
                Thread.Sleep(1000);
            }

            Console.WriteLine("Hello World!");
        }

        private static void Action(object sender, BasicDeliverEventArgs e)
        {
            Console.WriteLine("Recived");
            Debug.WriteLine("Recived");
        }
    }
}
