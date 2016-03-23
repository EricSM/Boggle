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
    class Model
    {
        public string CurrentUID = "";

        public static HttpClient CreateClient()
        {

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://bogglecs3500s16.azurewebsites.net/BoggleService.svc/");

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            return client;
        }

        public async void CreateUser(string NickName)
        {
            using (HttpClient client = CreateClient())
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

        public async void JoinGame(string UserToken, int TimeLimit)
        {
            using (HttpClient client = CreateClient())
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
        public async void CancelJoinRequest(string UserToken)
        {
            using (HttpClient client = CreateClient())
            {

                dynamic data = new ExpandoObject();
                data.UserToken = UserToken;
                StringContent content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");




            }

        }
    }
}
