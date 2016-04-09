﻿using System;
using System.Linq;
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
			//TODO: Use this method to return the value You want to bet
		    int bet = 0;
            var gameState = JsonConvert.DeserializeObject<GameState>(jsonState.ToString());

            try
            {
                Logger.LogHelper.Log("type=bet_begin action=bet_request request_id={0} game_id={1}", requestId, gameState.GameId);
                if (false)// gameState.OwnCards.Count() <= 2)
                {
                    // Pre Flop
                    Logger.LogHelper.Log("type=Pre Flop block action=bet_request request_id={0} game_id={1}", requestId, gameState.GameId);

                    if (gameState.HasPair())
                    {
                        Logger.LogHelper.Log("type=Pre Flop Has Pair action=bet_request request_id={0} game_id={1}", requestId, gameState.GameId);

                        bet += gameState.CurrentBuyIn + gameState.MinimumRaise - gameState.GetCurrentPlayer().Bet;
                    }
                    else if (gameState.CardsBySuit.Count() == 1)
                    {
                        Logger.LogHelper.Log("type=Pre Flop block action=bet_request request_id={0} game_id={1}", requestId, gameState.GameId);

                        var firstCard = gameState.CardsBySuit.Single().First();
                        var secondCard = gameState.CardsBySuit.Single().Last();

                        if (Math.Abs((int)firstCard.Rank - (int)secondCard.Rank) <= 2)
                        {
                            bet += gameState.CurrentBuyIn - gameState.GetCurrentPlayer().Bet;
                        }
                    }
                    else if (gameState.OwnCards.Any(card => (int)card.Rank >= 10))
                    {
                        bet += gameState.CurrentBuyIn - gameState.GetCurrentPlayer().Bet;
                    }
                }
                else
                {
                    // Post Flop

                    if (gameState.HasFlush())
                    {
                        bet = gameState.CurrentBuyIn + Math.Max(gameState.MinimumRaise, 1000) - gameState.GetCurrentPlayer().Bet;
                    }
                    else if (gameState.HasFourOfAKind())
                    {
                        bet = gameState.CurrentBuyIn + Math.Max(gameState.MinimumRaise, 800) - gameState.GetCurrentPlayer().Bet;

                    }
                    else if (gameState.HasThreeOfAKind())
                    {
                        bet = gameState.CurrentBuyIn + Math.Max(gameState.MinimumRaise, 200) - gameState.GetCurrentPlayer().Bet;

                    }
                    else if (gameState.HasPair())
                    {
                        bet = gameState.CurrentBuyIn + Math.Max(gameState.MinimumRaise, 50) - gameState.GetCurrentPlayer().Bet;

                    }
                    else
                    {
                        //if (new Random().Next()%10 == 0)
                        //{
                        //    bet = gameState.CurrentBuyIn + Math.Max(gameState.MinimumRaise, 50) -
                        //          gameState.GetCurrentPlayer().Bet;
                        //}
                        //else
                        //{
                        //    bet = 0;
                        //}
                    }
                }

                string cards = String.Join(",", gameState.OwnCards);
                Logger.LogHelper.Log("request_id={0} game_id={1} bet={2}, cards={3}", requestId, gameState.GameId, bet, cards);

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

