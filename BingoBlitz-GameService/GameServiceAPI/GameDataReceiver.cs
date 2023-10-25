using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;

namespace BingoBlitz_GameService
{
    public class GameDataReceiver
    {
        public Connector? Connector { get; private set; }
        public IModel Channel { get; private set; }

        public GameDataReceiver(Connector connector)
        {
            Connector = connector;

            Connector.ConnectionOpened += StartReceiving;

            if (Connector != null && Connector.Connection != null && Connector.Connection.IsOpen) 
            {
                StartReceiving();
            }
        }

        public void StartReceiving()
        {
            Channel = Connector.Connection.CreateModel();

            Channel.QueueDeclare(
                queue: "game_data",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            EventingBasicConsumer consumer = new(Channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine($" [x] Received '{message}'");

                Channel.BasicAck(ea.DeliveryTag, false);
            };

            string tag = Channel.BasicConsume("game_data", false, consumer);
        }
    }
}
