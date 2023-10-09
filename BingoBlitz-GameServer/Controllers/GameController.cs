using Microsoft.AspNetCore.Mvc;

namespace BingoBlitz_GameServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : ControllerBase
    {
        private readonly ILogger<GameController> _logger;

        public GameController(ILogger<GameController> logger)
        {
            _logger = logger;
        }

        [HttpPost(Name = "JoinGame")]
        public string JoinGame(string gameId)
        {
            if (gameId.Length != 6) return "No game found with the supplied code";

            return $"Joined game with ID {gameId}";
        }
    }
}