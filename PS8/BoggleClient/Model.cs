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
        
        public string CurrentUID = "";
        public string GameID = "";
        public int TimeLeft = -1;
        public string GameState;
        public string Board = "";
        public string Player1 = "Player 1";
        public string Player2 = "Player 2";
        public int Player1Score;
        public int Player2Score;
        //public Dictionary<string, float> Player1WordsPlayed = new Dictionary<string, float>();
        //public Dictionary<string, float> Player2WordsPlayed = new Dictionary<string, float>();
        public dynamic Player1WordsPlayed = new ExpandoObject();
        public dynamic Player2WordsPlayed = new ExpandoObject();

        public static HttpClient CreateClient(string serverName)
        {

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(serverName);

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            return client;
        }

        public void CreateUser(string NickName, string serverName)
        {
            using (HttpClient client = CreateClient(serverName))
            {

                dynamic data = new ExpandoObject();
                data.Nickname = NickName;

                StringContent content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync("users", content).Result;

                if (response.IsSuccessStatusCode)
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
                dynamic data = new ExpandoObject();
                data.UserToken = CurrentUID;
                data.TimeLimit = TimeLimit;
                Console.WriteLine(TimeLimit);
                StringContent content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

                HttpResponseMessage response = client.PostAsync("games", content).Result;

                if (response.IsSuccessStatusCode)
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

                dynamic data = new ExpandoObject();
                data.UserToken = CurrentUID;
                StringContent content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");


                HttpResponseMessage response = client.PutAsync("games", content).Result;
                Console.WriteLine(CurrentUID);
                if (response.IsSuccessStatusCode)
                {
                    String result = response.Content.ReadAsStringAsync().Result;
                    dynamic JSONoutput = JsonConvert.DeserializeObject(result);
                    GameState = "";
                    //Console.WriteLine(JSONoutput);
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

                dynamic data = new ExpandoObject();
                data.UserToken = CurrentUID;
                data.Word = word;
                StringContent content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");


                HttpResponseMessage response = client.PutAsync("games/"+GameID, content).Result;

                if (response.IsSuccessStatusCode)
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
                dynamic data = new ExpandoObject();
                data.UserToken = CurrentUID;


                HttpResponseMessage response = client.GetAsync("games/" + GameID + (brief ? "?Brief=yes" : "")).Result;

                if (response.IsSuccessStatusCode)
                {
                    String result = response.Content.ReadAsStringAsync().Result;
                    dynamic JSONoutput = JsonConvert.DeserializeObject(result);

                    GameState = JSONoutput.GameState;
                    if (brief)
                    {
                        TimeLeft = JSONoutput.TimeLeft;
                        Player1Score = JSONoutput.Player1.Score;
                        Player2Score = JSONoutput.Player2.Score;
                    }

                    if (GameState != "pending" && !brief)
                    {
                        TimeLeft = JSONoutput.TimeLeft;
                        Board = JSONoutput.Board;
                        Player1 = JSONoutput.Player1.Nickname;
                        Player2 = JSONoutput.Player2.Nickname;
                        Player1Score = JSONoutput.Player1.Score;
                        Player2Score = JSONoutput.Player2.Score;
                    }

                    if (GameState == "completed" && !brief)
                    {
                        Player1WordsPlayed = JSONoutput.Player1.WordsPlayed;
                        Player2WordsPlayed = JSONoutput.Player2.WordsPlayed;
                        //foreach (dynamic word in JSONoutput.Player1.WordsPlayed)
                        //{
                        //    try
                        //    {
                        //        Player1WordsPlayed.Add(word.Word, word.Score);
                        //        Console.WriteLine(word.Word);
                        //    }
                        //    catch
                        //    {

                        //    }

                        //}

                        //foreach (dynamic word in JSONoutput.Player2.WordsPlayed)
                        //{
                        //    try
                        //    {
                        //        Player1WordsPlayed.Add(word.Word, word.Score);
                        //        Console.WriteLine(word.Word);
                        //    }
                        //    catch
                        //    {

                        //    }
                        //}
                    }

                    Console.WriteLine(JSONoutput);
                }
            }
        }
    }
}