using System;
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
            throw new NotImplementedException();
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
                return UserToken;
            }
        }

        public GameStatus GetGameStatus(int gameID)
        {
            throw new NotImplementedException();
        }

        public string JoinGame(string userToken, int timeLimit)
        {

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
                games[gameID].Player2Token = userToken;
                string GameID = gameID.ToString();
                StartPendingGame(timeLimit);

                SetStatus(Created);
                return GameID;
            }
            else if (games[gameID].Player2Token != null || games[gameID].Player1Token == null)
            {
                games[gameID].Player1Token = userToken;
                string GameID = gameID.ToString();
                StartPendingGame(timeLimit);

                SetStatus(Created);
                return GameID;
            }
            else
            {
                games[gameID].Player1Token = userToken;
                games[gameID].TimeLimit = timeLimit;
                string GameID = gameID.ToString();
                                
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

        public void PlayWord(int gameID)
        {
            throw new NotImplementedException();
        }
    }
}
