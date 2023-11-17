using Newtonsoft.Json;

namespace BingoBlitz_CommunityHub.Models
{
    public class ObjectiveCollection
    {
        [JsonProperty("id")]
        public string? Id { get; set; }
        public string Name { get; set; }
        public List<Objective> Objectives { get; set; }
    }
}
