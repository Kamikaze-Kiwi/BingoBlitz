using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;

namespace BingoBlitz_GameServer.Controllers
{
    [ApiController]
    [Route("api/game/")]
    public class GameController : ControllerBase
    {
        private readonly ILogger<GameController> _logger;

        public GameController(ILogger<GameController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        [Route("join")]
        public string JoinGame(string gameId)
        {
            if (gameId.Length != 6) return "No game found with the supplied code";

            return $"Joined game with ID {gameId}";
        }

        [HttpPost]
        [Route("save")]
        public string SaveGame(string gameData)
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

            channel.QueueDeclare(queue: "game_data",
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            byte[] body = Encoding.UTF8.GetBytes(gameData);

            channel.BasicPublish(exchange: "",
                routingKey: "game_data",
                basicProperties: null,
                body: body);

            return $"Game data saved: {gameData}";
        }
    }
}