using CustomNetworking;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BoggleServer
{
    //Boggle Server Class
    public class BoggleServer
    {
        //Define variables
        private int timeLimit;
        private HashSet<String> dictionary;
        private String initialBoardSetup;
        private TcpListener server;
        private Player waitingForGame;
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
            catch(Exception e)
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
                    URL = m.Groups[2].Value;
                    Console.WriteLine("Method: " + m.Groups[1].Value);
                    Console.WriteLine("URL: " + m.Groups[2].Value);
                }
                if (s.StartsWith("Content-Length:"))
                {
                    if(method == "GET")
                    {
                        contentLength = 10;
                    }
                    else
                    {
                        contentLength = Int32.Parse(s.Substring(16).Trim());
                    }
                    
                }

                //Problem is that GET doesn't have an \r at the end. Why? Because it doesn't have any data!
                //But you haven't written any method to utilize get.
                Console.WriteLine("Current S: " + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(s)));
                if (s == "\r")
                {
                    Console.WriteLine("YES");
                    if(method == "GET")
                    {
                        ContentReceived(URL, null, null);
                    }
                    else
                    {
                        ss.BeginReceive(ContentReceived, null, contentLength);
                    }
                   
                }
                else
                {
                    ss.BeginReceive(LineReceived, null);
                    //For some unknown reason, GET requests will end up looping over here. Find out why!
                }
            }
        }

        private void ContentReceived(string s, Exception e, object payload)
        {
            Console.WriteLine("Start of ContentReceived");
            if (s != null)
            {
                //Any request other than GET is processed here

                // Call service method
                //Player p = JsonConvert.DeserializeObject<Player>(s);
                //string result =
                //    JsonConvert.SerializeObject(
                //            new Player { Name = "June", Score = 5 },
                //            new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                string result = "<font size='50'>One Small Step for a Man</font>";
                ss.BeginSend("HTTP/1.1 200 OK\n", Ignore, null);
                ss.BeginSend("Content-Type: text/html\n", Ignore, null);
                ss.BeginSend("Content-Length: " + result.Length + "\n", Ignore, null);
                ss.BeginSend("\r\n", Ignore, null);
                ss.BeginSend(result, (ex, py) => { ss.Shutdown(); }, null);

            }
            else
            {
                //GET requests don't have any input by user, so s will be null, but it's still ok
                string result = "123";
                ss.BeginSend("HTTP/1.1 200 OK\n", Ignore, null);
                ss.BeginSend("Content-Type: text/plain\n", Ignore, null);
                ss.BeginSend("Content-Length: " + result.Length + "\n", Ignore, null);
                ss.BeginSend("\r\n", Ignore, null);
                ss.BeginSend(result, (ex, py) => { ss.Shutdown(); }, null);
            }

        }

        private void Ignore(Exception e, object payload)
        {
        }
    }

    //Player Class
    public class Player
    {
        //String Socket
        public StringSocket Socket { get; private set; }

        //Player Name
        public String Name { get; set; }

        //Player Score
        public int Score { get; set; }

        //Unique Player Played Words (Legal)
        public HashSet<String> LegalWords { get; private set; }

        //Illegal Player Words
        public HashSet<String> IllegalWords { get; private set; }


        //Initialize Player Settings
        //public Player(StringSocket s, String name)
        //{
        //    // Socket Reference
        //    Socket = s;
        //    // Player Name
        //    Name = name;

        //    // Initialize Game Variables
        //    Score = 0; //Set score to 0
        //    LegalWords = new HashSet<String>(); //Legal words - empty
        //    IllegalWords = new HashSet<String>(); //Illegal words - empty
        //}
    }
}
