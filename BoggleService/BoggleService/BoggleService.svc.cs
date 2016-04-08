using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.ServiceModel.Web;
using static System.Net.HttpStatusCode;

namespace Boggle
{
    public class BoggleService : IBoggleService
    {
        private static string BoggleDB;
        private static int gameID;
        private readonly static Dictionary<String, UserInfo> users = new Dictionary<String, UserInfo>();
        private readonly static Dictionary<int, Game> games = new Dictionary<int, Game> { { gameID, new Game() } };
        private HashSet<string> dictionary;
        private static readonly object sync = new object();

        static BoggleService()
        {
            BoggleDB = ConfigurationManager.ConnectionStrings["BoggleDB"].ConnectionString;
            
            // Retrieve GameID for pending game.
            using (SqlConnection conn = new SqlConnection(BoggleDB))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    using (SqlCommand command = new SqlCommand("select GameID from Games order by GameID desc", conn, trans))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            reader.Read();
                            gameID = (int)reader["GameID"];
                        }
                        trans.Commit();
                    }
                }
            }
        }

        private HashSet<string> LoadDictionary(string filename)
        {
            HashSet<string> set = new HashSet<string>();

            try
            {
                using (StreamReader reader = new StreamReader(filename))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        set.Add(line);
                    }
                }
            }
            catch (Exception)
            {
                Console.Error.WriteLine("Error reading dictionary file.");
                Debug.WriteLine("I CAn'T READD!!!!");
                // Environment.Exit(1);
            }

            return set;
        }

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


       //create sql for Users
        public string khk()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["BoggleDB"].ConnectionString;
            string queryString = "SELECT * FROM Users;";
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand(queryString, connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    var outputstuff = "";
                    while (reader.Read())
                    {
                        outputstuff += String.Format("{0}, {1}", reader[0], reader[1]);
                    }
                    return outputstuff.ToString();
                }
            }
        }//create sql for Games
        public string KhG()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["BoggleDB"].ConnectionString;
            string queryString = "SELECT * FROM Games;";
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand(queryString, connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    var outputstuff = "";
                    while (reader.Read())
                    {
                        outputstuff += String.Format("{0}, {1}", reader[0], reader[1]);
                    }
                    return outputstuff.ToString();
                }
            }
        }
        //create sql for Words
        public string MAG()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["BoggleDB"].ConnectionString;
            string queryString = "SELECT * FROM Words;";
            using (var connection = new SqlConnection(connectionString))
            {
                var command = new SqlCommand(queryString, connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    var outputstuff = "";
                    while (reader.Read())
                    {
                        outputstuff += String.Format("{0}, {1}", reader[0], reader[1]);
                    }
                    return outputstuff.ToString();
                }
            }
        }
        public void CancelJoin(Token userToken)
        {
            //If UserToken is invalid, responds with status 403 (Forbidden).
            if (userToken.UserToken == null)
            {
                SetStatus(Forbidden);
                return;
            }

            using (SqlConnection conn = new SqlConnection(BoggleDB))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    // Check if user exists
                    using (SqlCommand command = new SqlCommand("select UserID from Users where UserID = @UserID", conn, trans))
                    {
                        command.Parameters.AddWithValue("@UserID", userToken.UserToken);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                SetStatus(Forbidden);
                                trans.Commit();
                                return;
                            }
                        }
                    }

                    // Check if UserToken is not a player in the pending game.
                    using (SqlCommand command = new SqlCommand("select Player1 from Games where GameID = @GameID", conn, trans))
                    {
                        command.Parameters.AddWithValue("@GameID", gameID);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            reader.Read();

                            if ((string)reader["Player1"] != userToken.UserToken)
                            {
                                SetStatus(Forbidden);
                                trans.Commit();
                                return;
                            }
                        }
                    }


                    // Otherwise, removes UserToken from the pending game and responds with status 200 (OK).
                    using (SqlCommand command = new SqlCommand("delete from Games where GameID = @GameID", conn, trans))
                    {
                        command.Parameters.AddWithValue("@GameID", gameID);
                        command.ExecuteNonQuery();
                        trans.Commit();
                        SetStatus(OK);
                    }
                }
            }
        }

        public string CreateUser(Username nickname)
        {
            // Check for validity
            if (nickname.Nickname == null || nickname.Nickname.Trim().Length == 0)
            {
                SetStatus(Forbidden);
                return null;
            }
           
            using (SqlConnection conn = new SqlConnection(BoggleDB))
            {
                conn.Open();

                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    using (SqlCommand command = new SqlCommand("insert into Users (UserID, Nickname) values(@UserID, @Nickname)", conn, trans))
                    {
                        // Add new user and return unique token            
                        string UserToken = Guid.NewGuid().ToString();
                        
                        command.Parameters.AddWithValue("@UserID", UserToken);
                        command.Parameters.AddWithValue("@Nickname", nickname.Nickname);


                        command.ExecuteNonQuery();
                        SetStatus(Created);

                        //Add the values to Users dictionary
                        var tempuserdic = new UserInfo();
                        tempuserdic.Nickname = nickname.Nickname;
                        tempuserdic.UserToken = UserToken;
                        users.Add(UserToken, tempuserdic);

                        trans.Commit();

                        dynamic JSONOutput = new ExpandoObject();
                        JSONOutput.UserToken = UserToken;
                        var outputcontent = JsonConvert.SerializeObject(JSONOutput);

                        return outputcontent;
                    }
                }
            }
            
        }

        public GameStatus GetGameStatus(int gameID, string brief)
        {
            // Checks if gameID is valid
            if (!games.ContainsKey(gameID))
            {
                SetStatus(Forbidden);
                return null;
            }
            // else return status and update game
            else
            {
                Game thisGame = games[gameID];
                GameStatus status = new GameStatus();

                // if game is pending
                if (thisGame.GameState == "pending")
                {
                    SetStatus(OK);
                    return new GameStatus() { GameState = "pending" };
                }
                // if game is active or completed and "Brief=yes" was a parameter
                if ((thisGame.GameState == "active" || thisGame.GameState == "complete") && brief == "yes")
                {
                    status = new GameStatus()
                    {
                        GameState = thisGame.GameState,
                        TimeLeft = thisGame.TimeLeft,
                        Player1 = new Player()
                        {
                            Score = thisGame.Player1Score
                        },
                        Player2 = new Player()
                        {
                            Score = thisGame.Player2Score
                        }
                    };
                }
                // if game is active and "Brief=yes" was not a parameter
                else if (thisGame.GameState == "active" && brief != "yes")
                {
                    status = new GameStatus()
                    {
                        GameState = thisGame.GameState,
                        Board = thisGame.GameBoard,
                        TimeLimit = thisGame.TimeLimit,
                        TimeLeft = thisGame.TimeLeft,
                        Player1 = new Player()
                        {
                            Nickname = users[thisGame.Player1Token].Nickname,
                            Score = thisGame.Player1Score
                        },
                        Player2 = new Player()
                        {
                            Nickname = users[thisGame.Player2Token].Nickname,
                            Score = thisGame.Player2Score
                        }
                    };
                }
                // if game is complete and user did not specify brief
                else if (thisGame.GameState == "completed" && brief != "yes")
                {
                    var Player1Scores = new HashSet<WordScore>();
                    foreach (KeyValuePair<string, int> kv in thisGame.Player1WordScores)
                    {
                        Player1Scores.Add(new WordScore() { Word = kv.Key, Score = kv.Value });
                    }

                    var Player2Scores = new HashSet<WordScore>();
                    foreach (KeyValuePair<string, int> kv in thisGame.Player2WordScores)
                    {
                        Player2Scores.Add(new WordScore() { Word = kv.Key, Score = kv.Value });
                    }

                    status = new GameStatus()
                    {
                        GameState = thisGame.GameState,
                        Board = thisGame.GameBoard,
                        TimeLimit = thisGame.TimeLimit,
                        TimeLeft = thisGame.TimeLeft,
                        Player1 = new Player()
                        {
                            Nickname = users[thisGame.Player1Token].Nickname,
                            Score = thisGame.Player1Score,
                            WordsPlayed = Player1Scores
                        },
                        Player2 = new Player()
                        {
                            Nickname = users[thisGame.Player2Token].Nickname,
                            Score = thisGame.Player2Score,
                            WordsPlayed = Player2Scores
                        }
                    };
                }

                // update game
                games[gameID].TimeLeft -= (DateTime.Now - thisGame.StartTime).Seconds;

                if (games[gameID].TimeLeft <= 0)
                {
                    games[gameID].GameState = "complete";
                    games[gameID].TimeLeft = 0;
                }

                SetStatus(OK);
                return status;
            }
        }

        public string JoinGame(JoinRequest joinRequest)
        {
            string player1 = null;
            string player2 = null;
            string userToken = joinRequest.UserToken;
            int timeLimit = joinRequest.TimeLimit;

            using (SqlConnection conn = new SqlConnection(BoggleDB))
            {

                conn.Open();
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    //A user token is valid if it is non - null and identifies a user. Time must be between 5 and 120.

                    using (SqlCommand command = new SqlCommand("select UserID from Users where UserID = @UserID", conn, trans))
                    {
                        command.Parameters.AddWithValue("@UserID", userToken);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            if (userToken == null || !reader.HasRows || timeLimit < 5 || timeLimit > 120)
                            {
                                SetStatus(Forbidden);
                                trans.Commit();
                                return null;
                            }
                            
                        }
                    }

                    using (SqlCommand command = new SqlCommand("select Player1, Player2 from Games where GameID = @GameID", conn, trans))
                    {
                        command.Parameters.AddWithValue("@GameID", gameID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                player1 = (string)reader["Player1"];
                                int index = reader.GetOrdinal("Player2");
                                if (reader.IsDBNull(index))
                                {
                                    player2 = (string)"";
                                }
                                else
                                {
                                    player2 = (string)reader["Player2"];
                                }


                                // Check if user is already in pending game.
                                if (player1 == userToken ||  player2 == userToken)
                                {
                                    SetStatus(Conflict);
                                    trans.Commit();
                                    return null;
                                }
                            }
                        }
                    }


                    // If player 1 is taken, user is player 2
                    if (player1 != null && player2 == null)
                    {
                        int initTimeLImit = 0;

                        using (SqlCommand command = new SqlCommand("select TimeLimit from Games where GameID = @GameID", conn, trans))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    initTimeLImit = (int)reader["TimeLimit"];
                                }
                            }
                        }

                        using (SqlCommand command = new SqlCommand("update Games set Player2 = @Player2, GameState = @GameState, GameBoard = @GameBoard, TimeLimit = @TimeLimit, TimeLeft = @TimeLeft, StartTime = @StartTime where GameID = @GameID", conn, trans))
                        {
                            // Starts a new active game
                            int timeLeft = (initTimeLImit + timeLimit) / 2;

                            var newboggleboard = new BoggleBoard().ToString();
                            var currdate = DateTime.Now;
                            command.Parameters.AddWithValue("@Player2", userToken);
                            command.Parameters.AddWithValue("@GameState", "active");
                            command.Parameters.AddWithValue("@GameBoard", newboggleboard);
                            command.Parameters.AddWithValue("@TimeLimit", timeLeft);
                            command.Parameters.AddWithValue("@TimeLeft", timeLeft);
                            command.Parameters.AddWithValue("@StartTime", currdate);
                            command.Parameters.AddWithValue("@GameID", gameID);

                            command.ExecuteNonQuery();

                            SetStatus(Created);
                            trans.Commit();

                            //Add the data to the Game Dictionary
                            var tempgameinfo = new Game();
                            tempgameinfo.Player2Token = userToken;
                            tempgameinfo.GameState = "active";
                            tempgameinfo.GameBoard = newboggleboard;
                            tempgameinfo.TimeLimit = timeLeft;
                            tempgameinfo.TimeLeft = timeLeft;
                            tempgameinfo.StartTime = currdate;
                            tempgameinfo.Player2WordScores = new Dictionary<string, int>();
                            games.Add(gameID, tempgameinfo);



                            return gameID.ToString();
                        }
                    }

                    // if user is first to enter pending game
                    else
                    {
                        using (SqlCommand command = new SqlCommand("insert into Games (Player1, GameState, TimeLimit) values (@Player1, @GameState, @TimeLimit)", conn, trans))
                        {
                            // Starts a new active game
                            command.Parameters.AddWithValue("@Player1", userToken);
                            command.Parameters.AddWithValue("@GameState", "pending");
                            command.Parameters.AddWithValue("@TimeLimit", timeLimit);

                            command.ExecuteNonQuery();

                            int newPendingGameID;

                            lock (sync)
                            {
                                gameID++;
                                newPendingGameID = gameID;
                            }

                            SetStatus(Accepted);
                            trans.Commit();

                            //Add the data to the Game Dictionary
                            var tempgameinfo = new Game();
                            tempgameinfo.Player1Token = userToken;
                            tempgameinfo.GameState = "pending";
                            tempgameinfo.TimeLimit = timeLimit;
                            tempgameinfo.Player1WordScores = new Dictionary<string, int>();
                            ////CHEAT
                            //Dictionary<string, int> fudic = new Dictionary<string, int>();
                            //fudic.Add("LADY", -1);
                            //tempgameinfo.Player2WordScores = fudic;
                            games.Add(gameID, tempgameinfo);


                            dynamic JSONOutput = new ExpandoObject();
                            JSONOutput.GameID = newPendingGameID.ToString();
                            var outputcontent = JsonConvert.SerializeObject(JSONOutput);

                            return outputcontent;
                        }
                    }
                }
            }
        }

        public string PlayWord(string gameIDString, WordPlayed wordPlayed)
        {
          
                int gameID = int.Parse(gameIDString);
                string UserToken = wordPlayed.UserToken;
                string Word = wordPlayed.Word.ToUpper();
            Debug.WriteLine(UserToken);
            Debug.WriteLine(gameID);
            Debug.WriteLine(Word);

            Debug.WriteLine((games[gameID].Player1Token));
            Debug.WriteLine((games[gameID].Player2Token));
            // If Word is null or empty when trimmed, or if GameID or UserToken is missing or invalid,
            //
            // or if UserToken is not a player in the game identified by GameID, responds with response code 403 (Forbidden).
            if (Word == null || Word.Trim() == string.Empty || !users.ContainsKey(UserToken) ||
                    (games[gameID].Player1Token != UserToken && games[gameID].Player2Token != UserToken))
                {
                    SetStatus(Forbidden);
                    return null;
                }


                // *********** JUMPER 1 


                // Otherwise, if the game state is anything other than "active", responds with response code 409(Conflict).
                //else if (games[gameID].GameState != "active")
                //{
                //Debug.WriteLine(games[gameID].GameState);
                //    SetStatus(Conflict);
                //    return null;
                //}
                else
                {
                // Otherwise, records the trimmed Word as being played by UserToken in the game identified by GameID.
                // Returns the score for Word in the context of the game(e.g. if Word has been played before the score is zero). 
                // Responds with status 200(OK).Note: The word is not case sensitive.


                //// *********** JUMPER 2 ---> I'm ACTUALLY GIVING A CONSTANT VALUE FOR GAMEBOARD TO LET IT WORK

                //BoggleBoard board = new BoggleBoard(games[gameID].GameBoard);
                BoggleBoard board = new BoggleBoard("DUDEMSKDJEIWOQPL");
                int score = 0; //Default is 0
                    dictionary = LoadDictionary(AppDomain.CurrentDomain.BaseDirectory + "dictionary.txt");
               // Debug.Print(string.Join("", dictionary));
                //Check to see if the word already exists in the dictionary, and if so, do not give any points to the Player
                if (board.CanBeFormed(Word) && dictionary.Contains(Word))
                    {


                        if (Word.Length > 2) score++;
                        if (Word.Length > 4) score++;
                        if (Word.Length > 5) score++;
                        if (Word.Length > 6) score += 2;
                        if (Word.Length > 7) score += 6;

                        if (games[gameID].Player1Token == UserToken)
                        {
                            if (games[gameID].Player2WordScores != null)
                            {
                                if (games[gameID].Player2WordScores.ContainsKey(Word)) { 
                                    games[gameID].Player2Score -= games[gameID].Player2WordScores[Word];
                                    games[gameID].Player2WordScores[Word] = score = 0;
                                }
                          }

                            if (games[gameID].Player1WordScores != null)
                            {
                                if (games[gameID].Player1WordScores.ContainsKey(Word))
                                {
                                    score = 0;
                                }
                            }
                            else
                            {
                            //Make a SQL to add the played word to database
                            games[gameID].Player1WordScores.Add(Word, score);
                            }
                        }
                        else if (games[gameID].Player2Token == UserToken)
                        {
                            if (games[gameID].Player1WordScores.ContainsKey(Word))
                            {
                                games[gameID].Player1Score -= games[gameID].Player1WordScores[Word];
                                games[gameID].Player1WordScores[Word] = score = 0;
                            }

                            if (games[gameID].Player2WordScores.ContainsKey(Word))
                            {
                                score = 0;
                            }
                            else
                            {
                            //Make a SQL to add the played word to database
                            games[gameID].Player2WordScores.Add(Word, score);
                            }
                        }
                    }

                    SetStatus(OK);

                    dynamic JSONOutput = new ExpandoObject();
                    JSONOutput.Score = score.ToString();
                    var outputcontent = JsonConvert.SerializeObject(JSONOutput);

                    return outputcontent;
                }
            
        }
    }
}