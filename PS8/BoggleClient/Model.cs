﻿using Newtonsoft.Json;
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

        public static HttpClient CreateClient(string serverName)
        {

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(serverName);

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            return client;
        }

        public  void CreateUser(string NickName, string serverName)
        {
            using (HttpClient client = CreateClient(serverName))
            {

                dynamic data = new ExpandoObject();
                data.Nickname = "Hassan";

                StringContent content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync("users", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    String result = response.Content.ReadAsStringAsync().Result;
                    dynamic JSONoutput = JsonConvert.DeserializeObject(result);
                    //Console.WriteLine(JSONoutput);
                    CurrentUID = JSONoutput.UserToken;
                }
                else
                {
                    Console.WriteLine("API Error: " + response.StatusCode);
                    Console.WriteLine(response.ReasonPhrase);
                }
            }
        }

        public  void JoinGame(string UserToken, int TimeLimit, string serverName)
        {
            using (HttpClient client = CreateClient(serverName))
            {
                dynamic data = new ExpandoObject();
                data.UserToken = UserToken;
                data.TimeLimit = TimeLimit;
                StringContent content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

                HttpResponseMessage response = client.PostAsync("games", content).Result;

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
        public  void CancelJoinRequest(string UserToken, string serverName)
        {
            using (HttpClient client = CreateClient(serverName))
            {

                dynamic data = new ExpandoObject();
                data.UserToken = UserToken;
                StringContent content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");




            }

        }
    }
}
