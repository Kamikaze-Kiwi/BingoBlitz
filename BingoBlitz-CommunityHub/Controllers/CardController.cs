using Microsoft.AspNetCore.Mvc;

namespace BingoBlitz_CommunityHub.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CardController : ControllerBase
    {
        private readonly ILogger<CardController> _logger;

        public CardController(ILogger<CardController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetCards")]
        public List<string> GetCards(int start = 0, int amount = 10)
        {
            List<string> cards = new();
            for(int i = start; i < start + amount; i++)
            {
                cards.Add($"{i}) <card name>");
            }
            return cards;
        }
    }
}