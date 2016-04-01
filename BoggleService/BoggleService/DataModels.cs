﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
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
        private string gameState = "pending";

        public string GameID { get; set; }

        public string GameState { get { return gameState; } set { gameState = value; } }

        public string Player1Token { get; set; }

        public string Player2Token { get; set; }

        public string GameBoard { get; set; }

        public int TimeLimit { get; set; }

        public DateTime StartTime { get; set; }

        public int TimeLeft { get; set; }

        public int Player1Score { get; set; }

        public int Player2Score { get; set; }

        public Dictionary<string, int> Player1WordScores { get; set; }

        public Dictionary<string, int> Player2WordScores { get; set; }
    }

    public class JoinRequest
    {
        public string UserToken { get; set; }

        public int TimeLimit { get; set; }
    }

    public class WordPlayed
    {
        public string UserToken { get; set; }

        public string Word { get; set; }
    }

    [DataContract]
    public class GameStatus
    {
        [DataMember]
        public string GameState { get; set; }

        [DataMember]
        public string GameBoard { get; set; }

        [DataMember]
        public int TimeLimit { get; set; }

        [DataMember]
        public int TimeLeft { get; set; }

        [DataMember]
        public Player Player1 { get; set; }

        [DataMember]
        public Player Player2 { get; set; }
         
    }

    public class Player
    {
        public string Nickname { get; set; }

        public int Score { get; set; }

        public HashSet<WordScore> WordsPlayed { get; set; }
    }

    public class WordScore
    {
        public string Word { get; set; }

        public int Score { get; set; }
    }
}