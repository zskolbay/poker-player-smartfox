using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Nancy.Simple
{
    [JsonObject]
    public class GameState
    {
        [JsonProperty("tournament_id")]
        public string TournamentId { get; set; }

        [JsonProperty("game_id")]
        public string GameId { get; set; }

        [JsonProperty("round")]
        public int Round { get; set; }

        [JsonProperty("bet_index")]
        public int BetIndex { get; set; }

        [JsonProperty("small_blind")]
        public int SmallBlind { get; set; }

        [JsonProperty("current_buy_in")]
        public int CurrentBuyIn { get; set; }

        [JsonProperty("pot")]
        public int Pot { get; set; }

        [JsonProperty("minimum_raise")]
        public int MinimumRaise { get; set; }

        [JsonProperty("dealer")]
        public int Dealer { get; set; }

        [JsonProperty("orbits")]
        public int Orbits { get; set; }

        [JsonProperty("in_action")]
        public int InAction { get; set; }

        [JsonProperty("players")]
        public IEnumerable<Player> Players { get; set; }

        [JsonProperty("community_cards")]
        public IEnumerable<Card> CommunityCards { get; set; }

        private Player _me;
        
        public Player GetCurrentPlayer()
        {
                return _me ?? (_me = Players.ElementAt(InAction));
            }

        private IEnumerable<Card> _ownCards;

        public IEnumerable<Card> OwnCards
        {
            get
            {
                return _ownCards ?? (_ownCards = CommunityCards.Concat(GetCurrentPlayer().HoleCards));
            }
        }

        private IEnumerable<IGrouping<Rank, Card>> _cardsByRank;
        
        public IEnumerable<IGrouping<Rank, Card>> CardsByRank
        {
            get
            {
                return _cardsByRank ?? (_cardsByRank = OwnCards.GroupBy(card => card.Rank));
            }
        }
        
        private IEnumerable<IGrouping<Suit, Card>> _cardsBySuit;

        public IEnumerable<IGrouping<Suit, Card>> CardsBySuit
        {
            get
            {
                return _cardsBySuit ?? (_cardsBySuit = OwnCards.GroupBy(card => card.Suit));
            }
        }

        private IEnumerable<IEnumerable<Card>> _getPairs;

        public IEnumerable<IEnumerable<Card>> GetPairs()
        {
            return _getPairs ?? (_getPairs = CardsByRank.Where(group => group.Count() == 2));
        }

        private IEnumerable<IEnumerable<Card>> _getThreeOfAKinds;

        public IEnumerable<IEnumerable<Card>> GetThreeOfAKinds()
        {
            return _getThreeOfAKinds ?? (_getThreeOfAKinds = CardsByRank.Where(group => group.Count() == 3));
        }

        private IEnumerable<IEnumerable<Card>> _getFourOfAKinds;

        public IEnumerable<IEnumerable<Card>> GetFourOfAKinds()
        {
            return _getFourOfAKinds ?? (_getFourOfAKinds = CardsByRank.Where(group => group.Count() == 4));
        }

        private IEnumerable<IEnumerable<Card>> _getFlushes;

        public IEnumerable<IEnumerable<Card>> GetFlushes()
        {
            return _getFlushes ?? (_getFlushes = CardsBySuit.Where(group => group.Count() == 5));
        }

        public bool HasPair()
        {
            return GetPairs().Any();
        }

        public bool HasThreeOfAKind()
        {
            return GetThreeOfAKinds().Any();
        }

        public bool HasFourOfAKind()
        {
            return GetFourOfAKinds().Any();
        }

        public bool HasFlush()
        {
            return GetFlushes().Any();
        }

        /// <summary>
        /// Minimális összeg ahhoz, hogy tartsuk a tétet
        /// </summary>
        /// <returns></returns>
        public int GetValueToCall()
        {
            return CurrentBuyIn - GetCurrentPlayer().Bet;
        }
    }
}
