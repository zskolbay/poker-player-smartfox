using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Nancy.Simple
{
    [JsonObject]
    public class Player
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonConverter(typeof(PlayerStatusConverter))]
        [JsonProperty("status")]
        public PlayerStatus Status { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("stack")]
        public int Stack { get; set; }

        [JsonProperty("bet")]
        public int Bet { get; set; }

        [JsonProperty("hole_cards")]
        public IEnumerable<Card> HoleCards { get; set; }

        public Player()
        {
            this.HoleCards = Enumerable.Empty<Card>();
        }
    }
}
