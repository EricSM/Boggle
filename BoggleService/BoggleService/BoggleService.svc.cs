// PS10 
// Meysam Hamel && Eric Miramontes
// CS 3500

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
        private static int pendingGameID;
        private static HashSet<string> dictionary;
        private static readonly object sync = new object();

        static BoggleService()
        {
            BoggleDB = ConfigurationManager.ConnectionStrings["BoggleDB"].ConnectionString;
            dictionary = LoadDictionary(AppDomain.CurrentDomain.BaseDirectory + "dictionary.txt");

            // Retrieve GameID for most recent game.
            using (SqlConnection conn = new SqlConnection(BoggleDB))
            {
                // Open a connection
                conn.Open();

                //Represents a Transact-SQL transaction to be made in a SQL Server database
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    using (SqlCommand command = new SqlCommand("select GameID from Games order by GameID desc", conn, trans))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            reader.Read();
                            pendingGameID = (int)reader["GameID"];
                        }
                        trans.Commit();
                    }
                }
            }
        }
        //use load dictioary to read file name 
        private static HashSet<string> LoadDictionary(string filename)
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
                // Open a connection
                conn.Open();

                //Represents a Transact-SQL transaction to be made in a SQL Server database
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
                                reader.Close();
                                trans.Commit();
                                return;
                            }
                        }
                    }

                    // Check if UserToken is not a player in the pending game.
                    using (SqlCommand command = new SqlCommand("select Player1, GameState from Games where GameID = @GameID", conn, trans))
                    {
                        command.Parameters.AddWithValue("@GameID", pendingGameID);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            reader.Read();

                            if ((string)reader["Player1"] != userToken.UserToken || (string)reader["GameState"] != "pending")
                            {
                                SetStatus(Forbidden);
                                reader.Close();
                                trans.Commit();
                                return;
                            }
                        }
                    }


                    // Otherwise, removes UserToken from the pending game and responds with status 200 (OK).
                    using (SqlCommand command = new SqlCommand("delete from Games where GameID = @GameID", conn, trans))
                    {
                        command.Parameters.AddWithValue("@GameID", pendingGameID);
                        command.ExecuteNonQuery();
                        trans.Commit();
                        SetStatus(OK);
                    }
                }
            }
        }
        // create user 
        public Token CreateUser(Username nickname)
        {
            // Check for validity
            if (nickname.Nickname == null || nickname.Nickname.Trim().Length == 0)
            {
                SetStatus(Forbidden);
                return null;
            }
            // Connect to the DB
            using (SqlConnection conn = new SqlConnection(BoggleDB))
            {
               // Open a connection
                conn.Open();

                //Represents a Transact-SQL transaction to be made in a SQL Server database
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    //insert userID
                    using (SqlCommand command = new SqlCommand("insert into Users (UserID, Nickname) values(@UserID, @Nickname)", conn, trans))
                    {
                        // Add new user and return unique token            
                        string UserToken = Guid.NewGuid().ToString();

                        command.Parameters.AddWithValue("@UserID", UserToken);
                        command.Parameters.AddWithValue("@Nickname", nickname.Nickname);

                        //gainst the connection and returns the number of rows affected
                        command.ExecuteNonQuery();
                        SetStatus(Created);

                        //Add the values to Users dictionary
                        var tempuserdic = new UserInfo();
                        tempuserdic.Nickname = nickname.Nickname;
                        tempuserdic.UserToken = UserToken;
                        //users.Add(UserToken, tempuserdic);

                        trans.Commit();

                        
                        return new Token() { UserToken = UserToken };
                    }
                }
            }

        }
        //Iformation about the game named by GameID
        public GameStatus GetGameStatus(string gID, string brief)
        {
            Game thisGame;
            string Player1, Player2;
            int Player1Score = 0, Player2Score = 0, gameID;
            var Player1Words = new List<WordScore>();
            var Player2Words = new List<WordScore>();
            GameStatus status = new GameStatus();
            if (!int.TryParse(gID, out gameID)) { SetStatus(Forbidden); return null; }

            // connect to DB
            using (SqlConnection conn = new SqlConnection(BoggleDB))
            {
               // Open a connection
                conn.Open();

                //Represents a Transact-SQL transaction to be made in a SQL Server database
                using (SqlTransaction trans = conn.BeginTransaction())
                {

                    // Create a command
                    using (SqlCommand command = new SqlCommand("select * from Games where GameID = @GameID", conn, trans))
                    {
                        command.Parameters.AddWithValue("@GameID", gameID);

                        // Execute the command and cycle through the DataReader object

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (!reader.HasRows)// Checks if gameID is valid
                            {
                                SetStatus(Forbidden);
                                reader.Close();
                                trans.Commit();
                                return null;
                            }
                            else // else return status and update game
                            {
                                reader.Read();
                                thisGame = new Game()
                                {
                                    GameID = gameID.ToString(),
                                    Player1Token = (string)reader["Player1"], 
                                    Player2Token = (reader["Player2"] == DBNull.Value) ? string.Empty : (string)reader["Player2"],
                                    GameBoard = (reader["Board"] == DBNull.Value) ? string.Empty : (string)reader["Board"],
                                    TimeLimit = (reader["TimeLimit"] == DBNull.Value) ? 0 : (int)reader["TimeLimit"],
                                    StartTime = (reader["StartTime"] == DBNull.Value) ? new DateTime() : (DateTime)reader["StartTime"],
                                    GameState = (string)reader["GameState"]
                                };
                            }
                        }
                    }

                    // if game is pending
                    if (thisGame.GameState == "pending")
                    {
                        SetStatus(OK);
                        return new GameStatus() { GameState = "pending" };
                    }



                    // Retrieve usernames of players
                    using (SqlCommand command = new SqlCommand("select Nickname from Users where UserID = @UserID", conn, trans))
                    {
                        command.Parameters.AddWithValue("@UserID", thisGame.Player1Token);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            reader.Read();

                            Player1 = (string)reader["Nickname"];
                        }
                    }
                    // Retrieve usernames of players
                    using (SqlCommand command = new SqlCommand("select Nickname from Users where UserID = @UserID", conn, trans))
                    {
                        command.Parameters.AddWithValue("@UserID", thisGame.Player2Token);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            reader.Read();

                            Player2 = (string)reader["Nickname"];
                        }
                    }

                    // Retrieve scores and words played for players 1 and 2
                    using (SqlCommand command = new SqlCommand("select Word, Score from Words where GameID = @GameID and Player = @Player", conn, trans))
                    {
                        command.Parameters.AddWithValue("@GameID", thisGame.GameID);
                        command.Parameters.AddWithValue("@Player", thisGame.Player1Token);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Player1Score += (int)reader["Score"];
                                Player1Words.Add(new WordScore() { Word = (string)reader["Word"], Score = (int)reader["Score"] });
                            }
                        }
                    }
                    using (SqlCommand command = new SqlCommand("select Word, Score from Words where GameID = @GameID and Player = @Player", conn, trans))
                    {
                        command.Parameters.AddWithValue("@GameID", thisGame.GameID);
                        command.Parameters.AddWithValue("@Player", thisGame.Player2Token);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Player2Score += (int)reader["Score"];
                                Player2Words.Add(new WordScore() { Word = (string)reader["Word"], Score = (int)reader["Score"] });
                            }
                        }
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
                                Score = Player1Score
                            },
                            Player2 = new Player()
                            {
                                Score = Player2Score
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
                                Nickname = Player1,
                                Score = Player1Score
                            },
                            Player2 = new Player()
                            {
                                Nickname = Player2,
                                Score = Player2Score
                            }
                        };
                    }
                    // if game is complete and user did not specify brief
                    else if (thisGame.GameState == "complete" && brief != "yes")
                    {
                        status = new GameStatus()
                        {
                            GameState = thisGame.GameState,
                            Board = thisGame.GameBoard,
                            TimeLimit = thisGame.TimeLimit,
                            TimeLeft = thisGame.TimeLeft,
                            Player1 = new Player()
                            {
                                Nickname = Player1,
                                Score = Player1Score,
                                WordsPlayed = Player1Words
                            },
                            Player2 = new Player()
                            {
                                Nickname = Player2,
                                Score = Player2Score,
                                WordsPlayed = Player2Words
                            }
                        };
                    }


                    // update game
                    thisGame.TimeLeft -= (DateTime.Now - thisGame.StartTime).Seconds;

                    if (thisGame.TimeLeft <= 0)
                    {
                        thisGame.GameState = "complete";
                        thisGame.TimeLeft = 0;
                    }

                    using (SqlCommand command = new SqlCommand("update Games set TimeLeft = @TimeLeft, GameState = @GameState where GameID = @GameID", conn, trans))
                    {
                        command.Parameters.AddWithValue("@GameState", thisGame.GameState);
                        command.Parameters.AddWithValue("@TimeLeft", thisGame.TimeLeft);
                        command.Parameters.AddWithValue("@GameID", thisGame.GameID);

                        command.ExecuteNonQuery();
                    }


                    SetStatus(OK);
                    trans.Commit();
                    return status;
                }
            }
        }

        // TODO keeps creating pending games
        public GID JoinGame(JoinRequest joinRequest)
        {
            string player1 = null;
            string player2 = null;
            string userToken = joinRequest.UserToken;
            int timeLimit = joinRequest.TimeLimit;

            using (SqlConnection conn = new SqlConnection(BoggleDB))
            {
                // Open a connection
                conn.Open();
                //Represents a Transact-SQL transaction to be made in a SQL Server database
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
                                reader.Close();
                                trans.Commit();
                                return null;
                            }

                        }
                    }

                    using (SqlCommand command = new SqlCommand("select Player1, Player2 from Games where GameID = @GameID", conn, trans))
                    {
                        command.Parameters.AddWithValue("@GameID", pendingGameID);

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
                                if (player1 == userToken || player2 == userToken)
                                {
                                    SetStatus(Conflict);
                                    reader.Close();
                                    trans.Commit();
                                    return null;
                                }
                            }
                        }
                    }


                    // If player 1 is taken, user is player 2
                    if (player1 != null && player2 == string.Empty)
                    {
                        int initTimeLImit = 0;

                        using (SqlCommand command = new SqlCommand("select TimeLimit from Games where GameID = @GameID", conn, trans))
                        {
                            command.Parameters.AddWithValue("@GameID", pendingGameID);
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    initTimeLImit = (int)reader["TimeLimit"];
                                }
                            }
                        }

                        using (SqlCommand command = new SqlCommand("update Games set Player2 = @Player2, GameState = @GameState, Board = @GameBoard, TimeLimit = @TimeLimit, TimeLeft = @TimeLeft, StartTime = @StartTime where GameID = @GameID", conn, trans))
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
                            command.Parameters.AddWithValue("@GameID", pendingGameID);

                            Debug.WriteLine(newboggleboard);

                            Debug.WriteLine(command.CommandText);

                            command.ExecuteNonQuery();

                            SetStatus(Created);
                            trans.Commit();


                            

                            return new GID() { GameID = pendingGameID.ToString() };
                        }
                    }

                    // if user is first to enter pending game
                    else
                    {
                        using (SqlCommand command = new SqlCommand("insert into Games (Player1, GameState, TimeLimit) output inserted.GameID values (@Player1, @GameState, @TimeLimit)", conn, trans))
                        {
                            // Starts a new active game
                            command.Parameters.AddWithValue("@Player1", userToken);
                            command.Parameters.AddWithValue("@GameState", "pending");
                            command.Parameters.AddWithValue("@TimeLimit", timeLimit);


                            lock (sync)
                            {
                                pendingGameID = (int)command.ExecuteScalar();
                            }

                            SetStatus(Accepted);
                            trans.Commit();


                            

                            return new GID() { GameID = pendingGameID.ToString() };
                        }
                    }
                }
            }
        }

        public PlayWordScore PlayWord(string gameIDString, WordPlayed wordPlayed)
        {

            int gameID;
            string userToken = wordPlayed.UserToken;
            string word = wordPlayed.Word.ToUpper();
            BoggleBoard board = new BoggleBoard();
            string gameState;
            if (!int.TryParse(gameIDString, out gameID)) { SetStatus(Forbidden); return null; }
            //Debug.WriteLine(userToken);
            //Debug.WriteLine(gameID);
            //Debug.WriteLine(word);

            using (SqlConnection conn = new SqlConnection(BoggleDB))
            {
                // Open a connection
                conn.Open();
                //Represents a Transact-SQL transaction to be made in a SQL Server database
                using (SqlTransaction trans = conn.BeginTransaction())
                {
                    // If Word is null or empty when trimmed, or if GameID or UserToken is missing or invalid,
                    // or if UserToken is not a player in the game identified by GameID, responds with response code 403 (Forbidden).
                    if (word == null || word.Trim() == string.Empty)
                    {
                        SetStatus(Forbidden);
                        return null;
                    }

                    //if UserToken is missing or invalid
                    using (SqlCommand command = new SqlCommand("select UserID from Users where UserID = @UserID", conn, trans))
                    {
                        command.Parameters.AddWithValue("@UserID", userToken);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                SetStatus(Forbidden);
                                reader.Close();
                                trans.Commit();
                                return null;
                            }
                        }

                    }
                    
                    using (SqlCommand command = new SqlCommand("select Player1, Player2, Board, GameState from Games where GameID = @GameID", conn, trans))
                    {
                        command.Parameters.AddWithValue("@GameID", gameID);

                        using (SqlDataReader reader = command.ExecuteReader())

                        {
                            // if GameID is missing or invalid
                            if (!reader.HasRows)
                            {
                                SetStatus(Forbidden);
                                reader.Close();
                                trans.Commit();
                                return null;
                            }

                            else
                            {
                                reader.Read();

                                int index = reader.GetOrdinal("GameState");
                                if (reader.IsDBNull(index))
                                {
                                    gameState = (string)"";
                                }
                                else
                                {
                                    gameState = (string)reader["GameState"];
                                }

                                if ((string)reader["Player1"] == userToken || (string)reader["Player2"] == userToken)
                                {
                                    //Player 1 has only joined, when player 2 joins is only when we get a board
                                    //Check to see if the Board value is null in database
                                    int index2 = reader.GetOrdinal("Board");
                                    if (reader.IsDBNull(index2))
                                    {
                                        //Board is null because only player 1 has joined
                                        //Return error that it's pending ********* Check to see if this is what we should give

                                        SetStatus(OK);
                                        return null;
                                    }
                                    else
                                    {
                                        board = new BoggleBoard((string)reader["Board"]);
                                    }


                                    //gameState = (string)reader["GameState"];
                                }
                                else //if UserToken is not a player in the game identified by GameID
                                {
                                    SetStatus(Forbidden);
                                    reader.Close();
                                    trans.Commit();
                                    return null;
                                }
                            }
                        }

                    }



                    // *********** JUMPER 1 


                    // Otherwise, if the game state is anything other than "active", responds with response code 409(Conflict).
                    if (gameState != "active")
                    {
                        Debug.WriteLine(gameState);
                        SetStatus(Conflict);
                        return null;
                    }

                    // Otherwise, records the trimmed Word as being played by UserToken in the game identified by GameID.
                    // Returns the score for Word in the context of the game(e.g. if Word has been played before the score is zero). 
                    // Responds with status 200(OK).Note: The word is not case sensitive.


                    //// *********** JUMPER 2 ---> I'm ACTUALLY GIVING A CONSTANT VALUE FOR GAMEBOARD TO LET IT WORK

                    // board = new BoggleBoard(games[gameID].GameBoard);
                    // board = new BoggleBoard("DUDEMSKDJEIWOQPL");

                    int score = 0; //Default is 0

                    // Debug.Print(string.Join("", dictionary));


                    var wordsPlayed = new List<string>();
                    // Retrieve all words user played this game
                    using (SqlCommand command = new SqlCommand("select Word from Words where GameID = @GameID and Player = @Player", conn, trans))
                    {
                        command.Parameters.AddWithValue("@GameID", gameID);
                        command.Parameters.AddWithValue("@Player", userToken);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                wordsPlayed.Add((string)reader["Word"]);
                            }
                        }
                    }

                    
                    //if string is less than 3 letters
                    if (word.Length < 3)
                    {
                        score = 0;
                    }
                    //else if word was already played
                    else if (wordsPlayed.Contains(word))
                    {
                        score = 0;
                    }
                    //else if the word already exists in the dictionary and can be formed in the board
                    else if (board.CanBeFormed(word) && dictionary.Contains(word))
                    {
                        if (word.Length > 2) score++;
                        if (word.Length > 4) score++;
                        if (word.Length > 5) score++;
                        if (word.Length > 6) score += 2;
                        if (word.Length > 7) score += 6;                        
                    }
                    else
                    {
                        score = -1;
                    }


                    using (SqlCommand command = new SqlCommand("insert into Words (Word, GameID, Player, Score) values(@Word, @GameID, @Player, @Score)", conn, trans))
                    {
                        command.Parameters.AddWithValue("@Word", word);
                        command.Parameters.AddWithValue("@GameID", gameID);
                        command.Parameters.AddWithValue("@Player", userToken);
                        command.Parameters.AddWithValue("@Score", score);

                        command.ExecuteNonQuery();
                        

                        

                        SetStatus(OK);
                        trans.Commit();
                        return new PlayWordScore() { Score = score.ToString() };
                    }                                        
                }
            }
        }
    }
}