﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.ServiceModel.Web;
using static System.Net.HttpStatusCode;

namespace Boggle
{
    public class BoggleService : IBoggleService
    {
        private static int gameID = 1;
        private readonly static Dictionary<String, UserInfo> users = new Dictionary<String, UserInfo>();
        private readonly static Dictionary<int, Game> games = new Dictionary<int, Game> { { gameID, new Game()} };
        private static readonly object sync = new object();


        /// <summary>
        /// The most recent call to SetStatus determines the response code used when
        /// an http response is sent.
        /// </summary>
        /// <param name="status"></param>
        private static void SetStatus(HttpStatusCode status)
        {
            WebOperationContext.Current.OutgoingResponse.StatusCode = status;
        }

        /// <summary>
        /// Returns a Stream version of index.html.
        /// </summary>
        /// <returns></returns>
        public Stream API()
        {
            SetStatus(OK);
            WebOperationContext.Current.OutgoingResponse.ContentType = "text/html";
            return File.OpenRead(AppDomain.CurrentDomain.BaseDirectory + "index.html");
        }

        public void CancelJoin(string userToken)
        {
            //If UserToken is invalid or is not a player in the pending game, responds with status 403 (Forbidden).
            if (userToken == null || !users.ContainsKey(userToken) || (games[gameID].Player1Token != userToken && games[gameID].Player2Token != userToken))
            {
                SetStatus(Forbidden);
            }
            else // Otherwise, removes UserToken from the pending game and responds with status 200 (OK).
            {
                lock (sync)
                {
                    games[gameID].Player1Token = null;
                }

                SetStatus(OK);
            }

        }

        public string CreateUser(string nickname)
        {
            if (nickname == null || nickname.Trim().Length == 0)
            {
                SetStatus(Forbidden);
                return null;
            }
            else
            {
                string UserToken = Guid.NewGuid().ToString();

                UserInfo userInfo = new UserInfo();
                userInfo.Nickname = nickname;
                userInfo.UserToken = UserToken;

                lock (sync)
                {
                    users.Add(UserToken, userInfo);
                }

                SetStatus(Created);
                return UserToken;
            }
        }

        public GameStatus GetGameStatus(int gameID, string brief)
        {
            throw new NotImplementedException();
        }

        public string JoinGame(JoinRequest joinRequest)
        {
            string userToken = joinRequest.UserToken;
            int timeLimit = joinRequest.TimeLimit;

            //A user token is valid if it is non - null and identifies a user.
            if (userToken == null || !users.ContainsKey(userToken) || timeLimit < 5 || timeLimit > 120)
            {
                SetStatus(Forbidden);
            }
            else if (games[gameID].Player1Token == userToken || games[gameID].Player2Token == userToken)
            {
                SetStatus(Conflict);
            }

            if (games[gameID].Player1Token != null && games[gameID].Player2Token == null)
            {
                string GameID = gameID.ToString();

                lock (sync)
                {
                    games[gameID].Player2Token = userToken;
                    StartPendingGame(timeLimit);
                }

                SetStatus(Created);
                return GameID;
            }
            else if (games[gameID].Player2Token != null || games[gameID].Player1Token == null)
            {
                string GameID = gameID.ToString();

                lock (sync)
                {
                    games[gameID].Player1Token = userToken;
                    StartPendingGame(timeLimit);
                }

                SetStatus(Created);
                return GameID;
            }
            else
            {
                string GameID = gameID.ToString();

                lock (sync)
                {
                    games[gameID].Player1Token = userToken;
                    games[gameID].TimeLimit = timeLimit;
                }
                                
                SetStatus(Accepted);
                return GameID;
            }

        }

        private void StartPendingGame(int timeLimit)
        {
            games[gameID].GameState = "active";
            games[gameID].GameBoard = new BoggleBoard().ToString();
            games[gameID].TimeLimit = (games[gameID].TimeLimit + timeLimit) / 2;
            games[gameID].StartTime = DateTime.Now;
            gameID++;
            games.Add(gameID, new Game());
        }

        public string PlayWord(int gameID, WordPlayed wordPlayed)
        {
            string UserToken = wordPlayed.UserToken;
            string Word = wordPlayed.Word;

            // If Word is null or empty when trimmed, or if GameID or UserToken is missing or invalid,
            // or if UserToken is not a player in the game identified by GameID, responds with response code 403 (Forbidden).
            if (Word == null || Word.Trim() == string.Empty || !users.ContainsKey(UserToken) ||
                (games[gameID].Player1Token != UserToken && games[gameID].Player2Token != UserToken))
            {
                SetStatus(Forbidden);
                return null;
            }
            // Otherwise, if the game state is anything other than "active", responds with response code 409(Conflict).
            else if (games[gameID].GameState != "active")
            {
                SetStatus(Conflict);
                return null;
            }
            else
            {
                // Otherwise, records the trimmed Word as being played by UserToken in the game identified by GameID.
                // Returns the score for Word in the context of the game(e.g. if Word has been played before the score is zero). 
                // Responds with status 200(OK).Note: The word is not case sensitive.
                BoggleBoard board = new BoggleBoard(games[gameID].GameBoard);
                int score = 0;

                // TODO Check if word exists in the dictionary
                if (board.CanBeFormed(Word))
                {
                    
                    if (Word.Length > 2) score++;
                    if (Word.Length > 4) score++;
                    if (Word.Length > 5) score++;
                    if (Word.Length > 6) score += 2;
                    if (Word.Length > 7) score += 6;

                    lock (sync)
                    {
                        if (games[gameID].Player1Token == UserToken)
                        {
                            if (games[gameID].Player1WordScores.ContainsKey(Word.ToUpper()))
                            {
                                score = 0;
                            }
                            else
                            {
                                games[gameID].Player1WordScores.Add(Word.ToUpper(), score);
                            }
                        }
                        else if (games[gameID].Player2Token == UserToken)
                        {
                            if (games[gameID].Player2WordScores.ContainsKey(Word.ToUpper()))
                            {
                                score = 0;
                            }
                            else
                            {
                                games[gameID].Player2WordScores.Add(Word.ToUpper(), score);
                            }
                        }
                    }                                        
                }

                SetStatus(OK);
                return score.ToString();
            }
        }
    }
}
