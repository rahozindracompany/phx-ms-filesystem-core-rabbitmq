using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;

namespace phx_ms_filesystem_core_rabbitmq
{
    class Receive
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = Protocols.DefaultProtocol.DefaultPort,
                UserName = "admin",
                Password = "admin",
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                // Receive filesystem microservice
                channel.QueueDeclare(queue: "crete_file_queueu",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    Console.WriteLine(" [x] Received {0}", message);
                    Thread.Sleep(500);
                };
                channel.BasicConsume(queue: "crete_file_queueu",
                                     autoAck: true,
                                     consumer: consumer);

                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
            }
        }
    }

}
