using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Boggle
{
    public class UserInfo
    {
        public string Nickname { get; set; }

        public string UserToken { get; set; }
    }

    public class Game
    {
        public string GameID { get; set; }

        public string GameState { get; set; }

        public string Player1Token { get; set; }

        public string Player2Token { get; set; }

        public string GameBoard { get; set; }

        public int TimeLimit { get; set; }

        public int TimeLeft { get; set; }

        public int Player1Score { get; set; }

        public int Player2Score { get; set; }

        public Dictionary<string, int> Player1WordScores { get; set; }

        public Dictionary<string, int> Player2WordScores { get; set; }
    }

    public class GameStatus
    {
        public string GameState { get; set; }

        public string GameBoard { get; set; }

        public int TimeLimit { get; set; }

        public int TimeLeft { get; set; }
         
        public Player Player1 { get; set; }

        public Player Player2 { get; set; }
         
    }

    public class Player
    {
        public string Nickname { get; set; }

        public int Score { get; set; }

        public Dictionary<string, int> WordsPlayed { get; set; }
    }
}