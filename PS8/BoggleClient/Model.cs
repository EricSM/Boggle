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
        public string CurrentUID = "";
        public string GameID = "";
        public string TimeLeft = "";
        public string GameState;
        public string Board = "";
        public string Player1;
        public string Player2;
        public int Player1Score;
        public int Player2Score;

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
                    TimeLeft = JSONoutput.TimeLeft;
                    Board = JSONoutput.Board;
                    Player1 = JSONoutput.Player1.Nickname;
                    Player2 = JSONoutput.Player2.Nickname;
                    Player1Score = JSONoutput.Player1.Score;
                    Player2Score = JSONoutput.Player2.Score;
                    

                    Console.WriteLine(JSONoutput);
                }
            }
        }
    }
}