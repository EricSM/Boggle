//Meysam Hamel & Eric Miramontes
//PS8

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BoggleClient
{
    public class Model
    {
        //The 16 char on the boggle game as a string 
        //private string board;
        
        public string CurrentUID = ""; // Player ID
        public string GameID = ""; // Current game ID
        public int TimeLeft = -1;
        public string GameState; // Pending, active or completed
        public string Board = ""; // The letters the dice rolled on
        public string Player1 = "Player 1"; // Gamertags
        public string Player2 = "Player 2";
        public int Player1Score;
        public int Player2Score;

        // Words both players played
        public dynamic Player1WordsPlayed = new ExpandoObject();
        public dynamic Player2WordsPlayed = new ExpandoObject();

        /// <summary>
        /// Creates an HttpClient for communicating with a boggle server.
        /// </summary>
        /// <param name="serverName">Boggle server</param>
        /// <returns></returns>
        public static HttpClient CreateClient(string serverName)
        {
            // Create client with given uri server name.
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(serverName);

            // Client accepts JSON
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            return client;
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="NickName">GamerTag</param>
        /// <param name="serverName">Boggle Server</param>
        public void CreateUser(string NickName, string serverName)
        {
            using (HttpClient client = CreateClient(serverName)) // Create HTTP Client
            {
                // Serialize data and send Post request to server
                dynamic data = new ExpandoObject();
                data.Nickname = NickName;

                StringContent content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync("users", content).Result;

                if (response.IsSuccessStatusCode) // If successful, deserialize and retrieve response data.
                {
                    String result = response.Content.ReadAsStringAsync().Result;
                    dynamic JSONoutput = JsonConvert.DeserializeObject(result);
                    Console.WriteLine(JSONoutput);
                    CurrentUID = JSONoutput.UserToken;
                }
                else
                {
                    Console.WriteLine("API Error: " + response.StatusCode);
                    Console.WriteLine(response.ReasonPhrase);
                }
            }
        }

        public void JoinGame(int TimeLimit, string serverName)
        {
            using (HttpClient client = CreateClient(serverName))
            {
                // Serialize data and send Post request to server
                dynamic data = new ExpandoObject();
                data.UserToken = CurrentUID;
                data.TimeLimit = TimeLimit;
                Console.WriteLine(TimeLimit);
                StringContent content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

                HttpResponseMessage response = client.PostAsync("games", content).Result;

                if (response.IsSuccessStatusCode) // If successful, deserialize and retrieve response data.
                {
                    String result = response.Content.ReadAsStringAsync().Result;
                    dynamic JSONoutput = JsonConvert.DeserializeObject(result);
                    GameID = JSONoutput.GameID;
                    Console.WriteLine(JSONoutput);
                }
                else
                {
                    Console.WriteLine("API Error: " + response.StatusCode);
                    Console.WriteLine(response.ReasonPhrase);
                }
            }
        }


        public void CancelJoinRequest(string serverName)
        {
            using (HttpClient client = CreateClient(serverName))
            {
                // Serialize data and send Put request to server
                dynamic data = new ExpandoObject();
                data.UserToken = CurrentUID;
                StringContent content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");


                HttpResponseMessage response = client.PutAsync("games", content).Result;
                Console.WriteLine(CurrentUID);
                if (response.IsSuccessStatusCode) // If successful, deserialize and retrieve response data.
                {
                    String result = response.Content.ReadAsStringAsync().Result;
                    dynamic JSONoutput = JsonConvert.DeserializeObject(result);
                    GameState = "";
                    Console.WriteLine(JSONoutput);
                }

                else
                {
                    Console.WriteLine("API Error: " + response.StatusCode);
                    Console.WriteLine(response.ReasonPhrase);
                }
            }
        }

        public void PlayWordRequest(string word, string serverName)
        {
            using (HttpClient client = CreateClient(serverName))
            {
                // Serialize data and send Put request to server
                dynamic data = new ExpandoObject();
                data.UserToken = CurrentUID;
                data.Word = word;
                StringContent content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");


                HttpResponseMessage response = client.PutAsync("games/"+GameID, content).Result;

                if (response.IsSuccessStatusCode) // If successful, deserialize and retrieve response data.
                {
                    String result = response.Content.ReadAsStringAsync().Result;
                    dynamic JSONoutput = JsonConvert.DeserializeObject(result);
                    Console.WriteLine(JSONoutput);
                }

                else
                {
                    Console.WriteLine("API Error: " + response.StatusCode);
                    Console.WriteLine(response.ReasonPhrase);
                }

            }
        }

        public void GameStatus(bool brief, string serverName)
        {

            using (HttpClient client = CreateClient(serverName))
            {
                // Serialize data and send Get request to server
                dynamic data = new ExpandoObject();
                data.UserToken = CurrentUID;


                HttpResponseMessage response = client.GetAsync("games/" + GameID + (brief ? "?Brief=yes" : "")).Result;

                if (response.IsSuccessStatusCode) // If successful, deserialize and retrieve response data.
                {
                    String result = response.Content.ReadAsStringAsync().Result;
                    dynamic JSONoutput = JsonConvert.DeserializeObject(result);

                    GameState = JSONoutput.GameState;
                    if (brief) // IF client requests brief information
                    {
                        TimeLeft = JSONoutput.TimeLeft;
                        Player1Score = JSONoutput.Player1.Score;
                        Player2Score = JSONoutput.Player2.Score;
                    }

                    // if client wants full information
                    if (GameState != "pending" && !brief) // if state is active or complete
                    {
                        TimeLeft = JSONoutput.TimeLeft;
                        Board = JSONoutput.Board;
                        Player1 = JSONoutput.Player1.Nickname;
                        Player2 = JSONoutput.Player2.Nickname;
                        Player1Score = JSONoutput.Player1.Score;
                        Player2Score = JSONoutput.Player2.Score;
                    }

                    if (GameState == "completed" && !brief) // if state is complete
                    {
                        Player1WordsPlayed = JSONoutput.Player1.WordsPlayed;
                        Player2WordsPlayed = JSONoutput.Player2.WordsPlayed;
                    }

                    Console.WriteLine(JSONoutput);
                }
            }
        }
    }
}