
// Meysam Hamel && Eric Miramontes
// CS 3500

using CustomNetworking;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Boggle
{
    //Boggle Server Class
    public class BoggleServer
    {
        //Define variables
        private static string BoggleDB;
        private int timeLimit;
        private HashSet<String> dictionary;
        private String initialBoardSetup;
        private TcpListener server;
        private Player waitingForGame;
        private static int pendingGameID;
        private readonly Object lockOnWaiting = new Object(); //For pending purposes
        private static string dictionarypath = AppDomain.CurrentDomain.BaseDirectory + @"dictionary.txt"; //Dictionary path

        //Main starting point
        //Remember to remove return comments at the end **** --> wherever error occurs there's a return and it will cause it to end
        static void Main(string[] args)
        {
            HashSet<String> dictionary;


            //Load the dictionry file

            // Check if File can be read correctly without exception
            if (BuildDictionary(dictionarypath, out dictionary))
            {
                // Check if File was empty
                if (dictionary.Count == 0)
                {
                    Console.Error.WriteLine("Dictionary file was empty.");
                    return;
                }

                Console.WriteLine("Dictionary loaded successfully.");
            }
            else
            {
                Console.Error.WriteLine("Error reading Dictionary file.");
                Console.Error.WriteLine("Dictionary file path: " + dictionarypath);
                return;
            }

            //Start the server
            new BoggleServer(60000);
            
            Console.Read();
        }

        //Check the dictionary file
        private static bool BuildDictionary(String filePath, out HashSet<String> dictionary)
        {
            // Initialize dictionary
            dictionary = new HashSet<string>();
            bool success = false;

            try
            {
                using (FileStream stream = File.Open(filePath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        while (!reader.EndOfStream)
                        {
                            dictionary.Add(reader.ReadLine().Trim().ToUpper());
                        }
                    }
                }

                success = true;
            }
            catch (Exception)
            {

            }

            return success;
        }


        //Boggle Server Function
        public BoggleServer(int portNumber)
        {

            //Create the socket server
            server = new TcpListener(IPAddress.Any, portNumber);

            //Start the server
            try
            {
                server.Start();

                //Start listening for incoming connections
                server.BeginAcceptSocket(ConnectionRequested, null);

                Console.WriteLine("Server successfully started.");
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Cannot start the server. " + e.Message);
            }


        }


        //Callback for incoming connection request
        private void ConnectionRequested(IAsyncResult ar)
        {
            Socket s = server.EndAcceptSocket(ar);
            server.BeginAcceptSocket(ConnectionRequested, null);
            new HttpRequest(new StringSocket(s, new UTF8Encoding()));
        }

        //Stops the server
        public void StopServer()
        {
            //Stop the TcpListener
            this.server.Stop();
            Console.WriteLine("Server halted.");
        }
    }


    //HTTP Request Class
    class HttpRequest
    {
        private StringSocket ss;
        private int lineCount;
        private int contentLength;
        private string URL;
        private string method;
        private BoggleService boggleService = new BoggleService();
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
                    method = m.Groups[1].Value;
                    URL = m.Groups[2].Value.ToLower();
                    Console.WriteLine("Method: " + m.Groups[1].Value);
                    Console.WriteLine("URL: " + m.Groups[2].Value);
                }

                if (s.StartsWith("Content-Length:"))
                {

                    contentLength = Int32.Parse(s.Substring(16).Trim());

                }


                if (s == "\r")
                {
                    //If the method is GET, don't even bother reading any user input; there isn't any!
                    if (method == "GET")
                    {
                        ContentReceived(URL, null, null);
                    }
                    //Other methods like POST do have user input, so catch them.
                    else
                    {
                        ss.BeginReceive(ContentReceived, null, contentLength);
                    }

                }
                else
                {
                    ss.BeginReceive(LineReceived, null);
                }
            }
        }

        private void ContentReceived(string s, Exception e, object payload)
        {
            //If any S is given --> regardless of GET or POST the S will be provided but sometimes it's empty
            if (s != null)
            {


                //Controller --> Determines what functions have be loaded given certain user requests and methods

                //Create a default result, which contains an error stating none of the following conditions matched
                string result;

                //Regex for /games/{GameID} - Playword
                Regex PW = new Regex(@"^\/boggleservice.svc\/games\/[0-9]+$");

                //Regex for /games/{GameID} - Playword 
                Regex GR = new Regex(@"^\/boggleservice.svc\/games\/[0-9]+(\?brief=yes)?$");
                //API Homepage
                if (method == "GET" && URL == "/")
                {

                    result = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "index.html");
                    ss.BeginSend("HTTP/1.1 200 OK\r\n", Ignore, null);
                    ss.BeginSend("Content-Type: text/html\r\n", Ignore, null);
                    ss.BeginSend("Content-Length: " + result.Length + "\r\n", Ignore, null);
                    ss.BeginSend("\r\n", Ignore, null);
                    ss.BeginSend(result, (ex, py) => { ss.Shutdown(); }, null);

                }
                //Create User
                else if (method == "POST" && URL == "/boggleservice.svc/users")
                {
                    //Remember, s is the input

                    result = "";

                    try
                    {

                        Username playeruser = JsonConvert.DeserializeObject<Username>(s);
                        result = boggleService.CreateUser(playeruser); //This is supposed to be the JSON output from the function

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    if (result == null)
                    {
                        result = "";
                    }

                    

                    //Display Results
                    ss.BeginSend("HTTP/1.1 "+ boggleService.GetStatusCode() +"\r\n", Ignore, null);
                    ss.BeginSend("Content-Type: application/json\r\n", Ignore, null);
                    ss.BeginSend("Content-Length: " + result.Length + "\r\n", Ignore, null);
                    ss.BeginSend("\r\n", Ignore, null);
                    ss.BeginSend(result, (ex, py) => { ss.Shutdown(); }, null);

                }
                //JoinGame
                else if (method == "POST" && URL == "/boggleservice.svc/games")
                {

                    //Run the Join Game Function

                    result = "";

                    try {

                        JoinRequest joingame = JsonConvert.DeserializeObject<JoinRequest>(s);
                        result = boggleService.JoinGame(joingame); //This is supposed to be the JSON output from the function

                    } catch(Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    if(result == null)
                    {
                        result = "";
                    }
                    //Display Results


                    ss.BeginSend("HTTP/1.1 " + boggleService.GetStatusCode() + "\r\n", Ignore, null);
                    ss.BeginSend("Content-Type: application/json\r\n", Ignore, null);
                    ss.BeginSend("Content-Length: " + result.Length + "\r\n", Ignore, null);
                    ss.BeginSend("\r\n", Ignore, null);
                    ss.BeginSend(result, (ex, py) => { ss.Shutdown(); }, null);

                }
                //CancelJoin
                else if (method == "PUT" && URL == "/boggleservice.svc/games")
                {

                    //Run the Cancel Join Function
                    Token canceljoin = JsonConvert.DeserializeObject<Token>(s);
                    boggleService.CancelJoin(canceljoin);

                    result = ""; //This is supposed to be the JSON output from the function
                    ss.BeginSend("HTTP/1.1 " + boggleService.GetStatusCode() + "\r\n", Ignore, null);
                    ss.BeginSend("Content-Type: application/json\r\n", Ignore, null);
                    ss.BeginSend("Content-Length: " + result.Length + "\r\n", Ignore, null);
                    ss.BeginSend("\r\n", Ignore, null);
                    ss.BeginSend(result, (ex, py) => { ss.Shutdown(); }, null);

                }
                //PlayWord
                else if (method == "PUT" && PW.IsMatch(URL))
                {

                    result = "";

                    try
                    {

                        //Run the Play Word Function
                        WordPlayed wordplayed = JsonConvert.DeserializeObject<WordPlayed>(s);
                        Regex gameIDregex = new Regex(@"([0-9]+)");
                        Match gameIDmatch = gameIDregex.Match(URL);
                        string gameID = gameIDmatch.Value;
                        Console.WriteLine("GAME ID : " + gameID);

                        result = boggleService.PlayWord(gameID, wordplayed); //This is supposed to be the JSON output from the function

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                    if (result == null)
                    {
                        result = "";
                    }
                    //
               

                    ss.BeginSend("HTTP/1.1 " + boggleService.GetStatusCode() + "\r\n", Ignore, null);
                    ss.BeginSend("Content-Type: application/json\r\n", Ignore, null);
                    //check this line as well
                    ss.BeginSend("Content-Length: " + result.Length + "\r\n", Ignore, null);
                    ss.BeginSend("\r\n", Ignore, null);
                    ss.BeginSend(result, (ex, py) => { ss.Shutdown(); }, null);

                }
                //GameStatus
                else if (method == "GET" && GR.IsMatch(URL))
                {
                    //Check to see if brief=yes is provided
                    string brief = "no";

                    var BR = new Regex(@"^\/boggleservice.svc\/games\/[0-9]+(\?brief=yes)$");

                    if (BR.IsMatch(URL))
                    {
                        brief = "yes";
                    }


                    Console.WriteLine(brief);

                    Regex gameIDregex = new Regex(@"([0-9]+)");
                    Match gameIDmatch = gameIDregex.Match(URL);
                    int gameID = Int32.Parse(gameIDmatch.Value);
                    Console.WriteLine("GAME ID : " + gameID);

                    //Run the Game Status Function [we supply the brief also]
                    var JSONOutput = JsonConvert.SerializeObject(boggleService.GetGameStatus(gameID, brief));


                    result = JSONOutput; //This is supposed to be the JSON output from the function
                    ss.BeginSend("HTTP/1.1 " + boggleService.GetStatusCode() + "\r\n", Ignore, null);
                    ss.BeginSend("Content-Type: application/json\r\n", Ignore, null);
                    ss.BeginSend("Content-Length: " + result.Length + "\r\n", Ignore, null);
                    ss.BeginSend("\r\n", Ignore, null);
                    ss.BeginSend(result, (ex, py) => { ss.Shutdown(); }, null);

                }
                //Nothing matched, throw an error
                else {
                    result = "<B>Error: no method matched. Result was unchanged. Sorry bro :(</B>";
                    ss.BeginSend("HTTP/1.1 400 BAD REQUEST\r\n", Ignore, null);
                    ss.BeginSend("Content-Type: text/html\r\n", Ignore, null);
                    ss.BeginSend("Content-Length: " + result.Length + "\r\n", Ignore, null);
                    ss.BeginSend("\r\n", Ignore, null);
                    ss.BeginSend(result, (ex, py) => { ss.Shutdown(); }, null);
                }
















                //Any request other than GET is processed here

                // Call service method
                //Player p = JsonConvert.DeserializeObject<Player>(s);
                //string result =
                //    JsonConvert.SerializeObject(
                //            new Player { Name = "June", Score = 5 },
                //            new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                //string result = "<font size='50'>One Small Step for a Man</font><BR><BR>I received input as: " +s;


                ss.BeginSend("HTTP/1.1 " + boggleService.GetStatusCode() + "\r\n", Ignore, null);
                ss.BeginSend("Content-Type: text/html\n", Ignore, null);
                ss.BeginSend("Content-Length: " + result.Length + "\n", Ignore, null);
                ss.BeginSend("\r\n", Ignore, null);
                ss.BeginSend(result, (ex, py) => { ss.Shutdown(); }, null);

            }
        }

        private void Ignore(Exception e, object payload)
        {
        }
    }

   
}
