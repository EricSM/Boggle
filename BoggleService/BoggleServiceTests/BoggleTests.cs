using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Dynamic;
using static System.Net.HttpStatusCode;
using System.Diagnostics;

namespace Boggle
{
    /// <summary>
    /// Provides a way to start and stop the IIS web server from within the test
    /// cases.  If something prevents the test cases from stopping the web server,
    /// subsequent tests may not work properly until the stray process is killed
    /// manually.
    /// </summary>
    public static class IISAgent
    {
        // Reference to the running process
        private static Process process = null;

        /// <summary>
        /// Starts IIS
        /// </summary>
        public static void Start(string arguments)
        {
            if (process == null)
            {
                ProcessStartInfo info = new ProcessStartInfo(Properties.Resources.IIS_EXECUTABLE, arguments);
                info.WindowStyle = ProcessWindowStyle.Minimized;
                info.UseShellExecute = false;
                process = Process.Start(info);
            }
        }

        /// <summary>
        ///  Stops IIS
        /// </summary>
        public static void Stop()
        {
            if (process != null)
            {
                process.Kill();
            }
        }
    }
    [TestClass]
    public class BoggleTests
    {
        /// <summary>
        /// This is automatically run prior to all the tests to start the server
        /// </summary>
        [ClassInitialize()]
        public static void StartIIS(TestContext testContext)
        {
            IISAgent.Start(@"/site:""BoggleService"" /apppool:""Clr4IntegratedAppPool"" /config:""..\..\..\.vs\config\applicationhost.config""");
        }

        /// <summary>
        /// This is automatically run when all tests have completed to stop the server
        /// </summary>
        [ClassCleanup()]
        public static void StopIIS()
        {
            IISAgent.Stop();
        }

        private RestTestClient client = new RestTestClient("http://localhost:60000/");

        
        [TestMethod]
        public void TestCreateUser1()
        {
            Response r = client.DoPostAsync("/users", "Gabe").Result;
            Assert.AreEqual(Created, r.Status);
            Assert.AreEqual(36, r.Data.Count);
        }

        [TestMethod]
        public void TestCreateUser2()
        {
            Response r = client.DoPostAsync("/users", null).Result;
            Assert.AreEqual(Forbidden, r.Status);
        }

        [TestMethod]
        public void TestJoinGame1()
        {
            Response r1 = client.DoPostAsync("/games", new JoinRequest() { UserToken = "abcd", TimeLimit = 30 }).Result;
            Assert.AreEqual(Forbidden, r1.Status);
        }

        [TestMethod]
        public void TestJoinGame2()
        {
            string userToken = client.DoPostAsync("/users", "joe").Result.Data;
            Response r1 = client.DoPostAsync("/games", new JoinRequest() { UserToken = userToken, TimeLimit = 30 }).Result;
            Assert.AreEqual(Accepted, r1.Status);

            Response r2 = client.DoPostAsync("/games", new JoinRequest() { UserToken = userToken, TimeLimit = 30 }).Result;
            Assert.AreEqual(Conflict, r1.Status);

            string userToken2 = client.DoPostAsync("/users", "conor").Result.Data;
            Response r3 = client.DoPostAsync("/games", new JoinRequest() { UserToken = userToken2, TimeLimit = 30 }).Result;
            Assert.AreEqual(Created, r3.Status);
        }

        [TestMethod]
        public void TestCancelJoin1()
        {
            Response r = client.DoPutAsync("/games", "abcd").Result;
            Assert.AreEqual(Forbidden, r.Status);
        }

        [TestMethod]
        public void TestCancelJoin2()
        {
            string userToken = client.DoPostAsync("/users", "fabian").Result.Data;
            Response r = client.DoPutAsync("/games", userToken).Result;
            Assert.AreEqual(OK, r.Status);
        }

        [TestMethod]
        public void TestPlayWord()
        {
            string userToken1 = client.DoPostAsync("/users", "john").Result.Data;
            Response r1 = client.DoPostAsync("/games", new JoinRequest() { UserToken = userToken1, TimeLimit = 30 }).Result;
            string userToken2 = client.DoPostAsync("/users", "smith").Result.Data;
            Response r2 = client.DoPostAsync("/games", new JoinRequest() { UserToken = userToken2, TimeLimit = 30 }).Result;

            Response r = client.DoPutAsync( new WordPlayed() { UserToken = userToken1, Word = null}, "/games/" + r1.Data.GameID).Result;
            Assert.AreEqual(Forbidden, r.Data);
        }
    }
}
