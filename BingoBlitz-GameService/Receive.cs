using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BingoBlitz_GameService
{
    public class Receive
    {
        public void StartReceiving()
        {
            ConnectionFactory factory = new()
            {
                UserName = "guest",
                Password = "guest",
                VirtualHost = "/",
                HostName = "rabbitmq",
                Port = 5672
            };

            using IConnection connection = factory.CreateConnection();
            using IModel channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: "game_data",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            EventingBasicConsumer consumer = new(channel);

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
