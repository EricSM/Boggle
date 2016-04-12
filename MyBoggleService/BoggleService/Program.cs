using CustomNetworking;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace Boggle
{
    public class WebServer
    {
        public static void Main()
        {
            new WebServer();
            Console.Read();
        }

        private TcpListener server;

        public WebServer()
        {
            server = new TcpListener(IPAddress.Any, 60000);
            server.Start();
            server.BeginAcceptSocket(ConnectionRequested, null);
        }

        private void ConnectionRequested(IAsyncResult ar)
        {
            Socket s = server.EndAcceptSocket(ar);
            server.BeginAcceptSocket(ConnectionRequested, null);
            new HttpRequest(new StringSocket(s, new UTF8Encoding()));
        }
    }

    class HttpRequest
    {
        private StringSocket ss;
        private int lineCount;
        private int contentLength;
        private string requestedfunction = "none";

        public HttpRequest(StringSocket stringSocket)
        {
            this.ss = stringSocket;
            ss.BeginReceive(LineReceived, null);
        }

        private void LineReceived(string s, Exception e, object payload)
        {
            lineCount++;
            Console.WriteLine(s);
            if (s != null)
            {
                if (lineCount == 1)
                {
                    Regex r = new Regex(@"^(\S+)\s+(\S+)");
                    Match m = r.Match(s);
                    Console.WriteLine("Method: " + m.Groups[1].Value);
                    Console.WriteLine("URL: " + m.Groups[2].Value);


                    //Regex for /games/{GameID}
                    Regex GR = new Regex(@"^\/games\/[0-9]+$");

                    //API Homepage Requested [Not Working Right Now]
                    if (m.Groups[1].Value == "GET" && m.Groups[2].Value == "/")
                    {
                        Console.WriteLine("** HOMEPAGE REQUESTED [F1]");
                        requestedfunction = "homepage";


                    }

                    //CreateUser Requested
                    else if (m.Groups[1].Value == "POST" && m.Groups[2].Value == "/users")
                    {
                        Console.WriteLine("** USER REQUESTED A NEW ACCOUNT [F1]");
                        requestedfunction = "createuser";
                    }

                    //JoinGame Requested
                    else if (m.Groups[1].Value == "POST" && m.Groups[2].Value == "/games")
                    {
                        Console.WriteLine("** USER REQUESTED A NEW ACCOUNT [F1]");
                        requestedfunction = "joingame";
                    }

                    //CancelJoin Requested
                    else if (m.Groups[1].Value == "PUT" && m.Groups[2].Value == "/games")
                    {
                        Console.WriteLine("** USER REQUESTED A NEW ACCOUNT [F1]");
                        requestedfunction = "canceljoin";
                    }

                    //PlayWord Requested
                    else if (m.Groups[1].Value == "PUT" && GR.IsMatch(m.Groups[2].Value))
                    {
                        Console.WriteLine("** GAME ID GIVEN -");
                        requestedfunction = "playword";
                    }

                    //GameStatus Requested [Not Working Right Now]
                    //else if (m.Groups[1].Value == "GET" && GR.IsMatch(m.Groups[2].Value))
                    //{
                    //    Console.WriteLine("** GAME ID GIVEN -");
                    //    requestedfunction = "gamestatus";
                    //}

                }
                if (s.StartsWith("Content-Length:"))
                {
                    contentLength = Int32.Parse(s.Substring(16).Trim());
                }
                if (s == "\r")
                {
                    //Get doesn't pass this if -- all other methods do! Get loops into the next "else"
                    ss.BeginReceive(ContentReceived, null, contentLength);
                }
                else
                {
                    ss.BeginReceive(LineReceived, null);
                }
            }
        }

        private void ContentReceived(string s, Exception e, object payload)
        {
            if (s != null)
            {
                Debug.WriteLine(requestedfunction);
                Debug.WriteLine(s); // This contains the user information (like given TimeLimit, etc.) in JSON; We need to decode it

                if (requestedfunction == "homepage")
                {
                    //Not Working Right Now
                    //API Homepage Requested
                    Console.WriteLine("** Homepage Requested");
                    ////string result = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "index.html");
                    string result = "<HTML>FUCK YOU</HTML>";

                    ss.BeginSend("HTTP/1.1 200 OK\n", Ignore, null);
                    ss.BeginSend("Content-Type: text/html\n", Ignore, null);
                    ss.BeginSend("Content-Length: " + result.Length + "\n", Ignore, null);
                    ss.BeginSend("\r\n", Ignore, null);
          
                    ss.BeginSend(result, (ex, py) => { ss.Shutdown(); }, null);
                    ////Run  whatever homepage function you have
                    ////return File.OpenRead(AppDomain.CurrentDomain.BaseDirectory + "index.html");

                }
                else if (requestedfunction == "createuser")
                {
                    //Given JSON is the variable s
                    var JSONInput = JsonConvert.DeserializeObject<dynamic>(s);

                    // Figure out somehow to call Createuser and pass the nickname....
                    //The result would be whatever JSON the function outputs, and we need to set it as the "string result" below.
                    //The server will then pass it to the user.
                    //var Response = Result.CreateUser(JSONInput.Nickname);

                    Console.WriteLine("** Create User Requested");
                    Person p = JsonConvert.DeserializeObject<Person>(s);
                    Console.WriteLine(p.Name + " " + p.Eyes);

                    // Call service method

                    string result =
                        JsonConvert.SerializeObject(
                                new Person { Name = "Mashti" + JSONInput.Nickname, Eyes = "Blue" },
                                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                    ss.BeginSend("HTTP/1.1 200 OK\n", Ignore, null);
                    ss.BeginSend("Content-Type: application/json\n", Ignore, null);
                    ss.BeginSend("Content-Length: " + result.Length + "\n", Ignore, null);
                    ss.BeginSend("\r\n", Ignore, null);
                    ss.BeginSend(result, (ex, py) => { ss.Shutdown(); }, null);
                    //Run  CreateUser(Username nickname);
                }
                else if (requestedfunction == "canceljoin")
                {
                    Console.WriteLine("** Cancel Join Requested");
                    Person p = JsonConvert.DeserializeObject<Person>(s);
                    Console.WriteLine(p.Name + " " + p.Eyes);

                    // Call service method

                    string result =
                        JsonConvert.SerializeObject(
                                new Person { Name = "June", Eyes = "Blue" },
                                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                    ss.BeginSend("HTTP/1.1 200 OK\n", Ignore, null);
                    ss.BeginSend("Content-Type: application/json\n", Ignore, null);
                    ss.BeginSend("Content-Length: " + result.Length + "\n", Ignore, null);
                    ss.BeginSend("\r\n", Ignore, null);
                    ss.BeginSend(result, (ex, py) => { ss.Shutdown(); }, null);
                    //Run  CreateUser(Username nickname);
                }
                else if (requestedfunction == "joingame")
                {
                    Console.WriteLine("** Join Game Requested");
                    Person p = JsonConvert.DeserializeObject<Person>(s);
                    Console.WriteLine(p.Name + " " + p.Eyes);

                    // Call service method

                    string result =
                        JsonConvert.SerializeObject(
                                new Person { Name = "June", Eyes = "Blue" },
                                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                    ss.BeginSend("HTTP/1.1 200 OK\n", Ignore, null);
                    ss.BeginSend("Content-Type: application/json\n", Ignore, null);
                    ss.BeginSend("Content-Length: " + result.Length + "\n", Ignore, null);
                    ss.BeginSend("\r\n", Ignore, null);
                    ss.BeginSend(result, (ex, py) => { ss.Shutdown(); }, null);
                    //Run  CreateUser(Username nickname);
                }
                else if (requestedfunction == "playword")
                {
                    Console.WriteLine("** Play Word Requested");
                    Person p = JsonConvert.DeserializeObject<Person>(s);
                    Console.WriteLine(p.Name + " " + p.Eyes);

                    // Call service method

                    string result =
                        JsonConvert.SerializeObject(
                                new Person { Name = "June", Eyes = "Blue" },
                                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                    ss.BeginSend("HTTP/1.1 200 OK\n", Ignore, null);
                    ss.BeginSend("Content-Type: application/json\n", Ignore, null);
                    ss.BeginSend("Content-Length: " + result.Length + "\n", Ignore, null);
                    ss.BeginSend("\r\n", Ignore, null);
                    ss.BeginSend(result, (ex, py) => { ss.Shutdown(); }, null);
                    //Run  CreateUser(Username nickname);
                }
                else if (requestedfunction == "gamestatus")
                {
                    //Not Working Right Now
                    Console.WriteLine("** Game Status Requested");
                    Person p = JsonConvert.DeserializeObject<Person>(s);
                    Console.WriteLine(p.Name + " " + p.Eyes);

                    // Call service method

                    string result =
                        JsonConvert.SerializeObject(
                                new Person { Name = "June", Eyes = "Blue" },
                                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                    ss.BeginSend("HTTP/1.1 200 OK\n", Ignore, null);
                    ss.BeginSend("Content-Type: application/json\n", Ignore, null);
                    ss.BeginSend("Content-Length: " + result.Length + "\n", Ignore, null);
                    ss.BeginSend("\r\n", Ignore, null);
                    ss.BeginSend(result, (ex, py) => { ss.Shutdown(); }, null);
                    //Run  CreateUser(Username nickname);
                }
                else if (requestedfunction == "none")
                {
                    //Error in Request -- Nothing was given
                    string result = "Error my friend! You didn't give me anything!";
                    ss.BeginSend("HTTP/1.1 200 OK\n", Ignore, null);
                    ss.BeginSend("Content-Type: text/html\n", Ignore, null);
                    ss.BeginSend("Content-Length: " + result.Length + "\n", Ignore, null);
                    ss.BeginSend("\r\n", Ignore, null);
                    ss.BeginSend(result, (ex, py) => { ss.Shutdown(); }, null);
                }

            }
        }

        private void Ignore(Exception e, object payload)
        {
        }
    }

    public class Person
    {
        public String Name { get; set; }
        public String Eyes { get; set; }
    }
}