using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BingoBlitz_GameService
{
    public class Connector
    {
        private readonly ConnectionFactory connectionFactory;
        public IConnection? Connection { get; private set; }

        /// <summary>
        /// This event fires every time a connection is established
        /// </summary>
        public event EmptyEventHandler? ConnectionOpened;

        public Connector(string userName, string password, string virtualHost, string hostName, int port)
        {
            connectionFactory = new()
            {
                UserName = userName,
                Password = password,
                VirtualHost = virtualHost,
                HostName = hostName,
                Port = port
            };

            _ = ContinuouslyConnect(1000);
        }

        /// <summary>
        /// Contiuously tries to connect until a connection has been established.
        /// </summary>
        /// <param name="delay">The time between each connection attempt in milliseconds.</param>
        public async Task ContinuouslyConnect(int delay)
        {
            while (!OpenConnection())
            {
                Console.WriteLine($"Failed to open a connection. Trying again in {delay} milliseconds.");

                await Task.Delay(delay);
            }

            Console.WriteLine("Succesfully connected!");
        }

        /// <summary>
        /// Opens a connection to a RabbitMQ container using <see cref="connectionFactory"/>.
        /// </summary>
        /// <returns>True if the connection was succesful, or an existing connection is already open. False if it could not create a connection.</returns>
        public bool OpenConnection()
        {
            if (Connection == null || !Connection.IsOpen)
            {
                try
                {
                    Connection = connectionFactory.CreateConnection();
                    Connection.ConnectionShutdown += ConnectionShutdown; //attach the shutdown handler to the new connection instance.
                    ConnectionOpened?.Invoke();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        private void ConnectionShutdown(object? sender, ShutdownEventArgs e)
        {
            Connection?.Abort();
            Connection = null;
            Console.WriteLine($"The connection was shut down. Reason: {e.ReplyText}");

            _ = ContinuouslyConnect(1000);
        }
    }

    public delegate void EmptyEventHandler();
}
