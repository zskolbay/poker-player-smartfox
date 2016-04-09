using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Nancy.Simple
{
	public static class PokerPlayer
	{
        private static Guid requestId = Guid.NewGuid();
        public static Guid RequestId { get { return requestId; } }
        public static Guid GenerateRequestId()
        {
            return (requestId = Guid.NewGuid());
        }


		public static readonly string VERSION = "Default C# folding player";

		public static int BetRequest(JObject jsonState)
		{
		    int bet = 0;
            var gameState = JsonConvert.DeserializeObject<GameState>(jsonState.ToString());

            try
            {
                //TODO: Use this method to return the value You want to bet
                Logger.LogHelper.Log("type=bet_begin action=bet_request request_id={0} game_id={1}", requestId, gameState.GameId);
                bet = new Random().Next() % 2 == 0 ? 100 : 0;
            }
            catch (Exception ex)
            {
                Logger.LogHelper.Error("type=error action=bet_request request_id={0} game_id={1} error_message={2}", requestId, gameState.GameId, ex);
            }

            return bet;
		}

		public static void ShowDown(JObject gameState)
		{
			//TODO: Use this method to showdown
		}
	}
}

