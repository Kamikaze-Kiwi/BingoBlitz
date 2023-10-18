using Microsoft.AspNetCore.Mvc;

namespace BingoBlitz_GameService.Controllers
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

        [HttpPost(Name = "SaveGame")]
        public string JoinGame(string gameData)
        {
            return $"Succesfully saved the game.";
        }
    }
}