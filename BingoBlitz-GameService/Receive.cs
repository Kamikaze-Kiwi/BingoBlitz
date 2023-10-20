using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BingoBlitz_GameService
{
    public class Receive
    {
        public void StartReceiving()
        {
            var factory = new ConnectionFactory();
            factory.UserName = "guest";
            factory.Password = "guest";
            factory.VirtualHost = "/";
            factory.HostName = "rabbitmq";
            factory.Port = 5672;
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "game_data",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);
                
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine($" [x] Received {message}");

                channel.BasicAck(ea.DeliveryTag, false);
            };

            string tag = channel.BasicConsume("game_data", false, consumer);
        }
    }
}
