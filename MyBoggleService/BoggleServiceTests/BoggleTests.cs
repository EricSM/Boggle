using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Dynamic;
using static System.Net.HttpStatusCode;
using System.Diagnostics;

namespace Boggle
{
    [TestClass]
    public class BoggleTests
    {
        private static System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
        private RestTestClient client = new RestTestClient("http://localhost:60000/");
        //private RestTestClient client = new RestTestClient("http://bogglecs3500s16.azurewebsites.net/");
        
        [TestMethod]
        public void TestCreateUser1()
        {
            new BoggleServer(60000);

            dynamic expando = new ExpandoObject();
            expando.Nickname = "Gabe";
            Response r = client.DoPostAsync("users", expando).Result;
            Assert.AreEqual(Created, r.Status);
        }

        [TestMethod]
        public void TestCreateUser2()
        {
            dynamic expando = new ExpandoObject();
            expando.Nickname = null;
            Response r = client.DoPostAsync("users", expando).Result;
            Assert.AreEqual(Forbidden, r.Status);
        }
        
        [TestMethod]
        public void TestJoinGame1()
        {
            Response r1 = client.DoPostAsync("games", new JoinRequest() { UserToken = "abcd", TimeLimit = 30 }).Result;
            Assert.AreEqual(Forbidden, r1.Status);
        }

        [TestMethod]
        public void TestJoinGame2()
        {
            dynamic expando = new ExpandoObject();
            expando.Nickname = "joe";
            string userToken = client.DoPostAsync("users", expando).Result.Data;

            Response r1 = client.DoPostAsync("games", new JoinRequest() { UserToken = userToken, TimeLimit = 30 }).Result;
            Assert.AreEqual(Accepted, r1.Status);

            Response r2 = client.DoPostAsync("games", new JoinRequest() { UserToken = userToken, TimeLimit = 30 }).Result;
            Assert.AreEqual(Conflict, r2.Status);


            dynamic expando2 = new ExpandoObject();
            expando2.Nickname = "conor";
            string userToken2 = client.DoPostAsync("users", expando2).Result.Data;

            Response r3 = client.DoPostAsync("games", new JoinRequest() { UserToken = userToken2, TimeLimit = 30 }).Result;
            Assert.AreEqual(Created, r3.Status);
        }
        
        [TestMethod]
        public void TestCancelJoin1()
        {
            dynamic expando = new ExpandoObject();
            expando.UserToken = "abcd";

            Response r = client.DoPutAsync(expando, "games").Result;
            Assert.AreEqual(Forbidden, r.Status);
        }



   

        [TestMethod]
        public void TestCancelJoin2()
        {
            dynamic expando = new ExpandoObject();

            expando.UserToken = client.DoPostAsync("users", new Username() { Nickname = "fabian" }).Result.Data;

            Response r1 = client.DoPostAsync("games", new JoinRequest() { UserToken = expando.UserToken, TimeLimit = 30 }).Result;
            Assert.AreEqual(Accepted, r1.Status);

            Response r2 = client.DoPutAsync(expando, "games").Result;
            Assert.AreEqual(OK, r2.Status);
        }
        
        [TestMethod]
        public void TestPlayWord()
        {
            string userToken1 = client.DoPostAsync("users", new Username() { Nickname = "john" }).Result.Data;
            Response r1 = client.DoPostAsync("games", new JoinRequest() { UserToken = userToken1, TimeLimit = 30 }).Result;
            

            string userToken2 = client.DoPostAsync("users", new Username(){ Nickname = "smith" }).Result.Data;
            Response r2 = client.DoPostAsync("games", new JoinRequest() { UserToken = userToken2, TimeLimit = 30 }).Result;
            Debug.WriteLine("test");
           
            string gameID = r1.Data.ToString();
            Response r3 = client.DoPutAsync( new WordPlayed() { UserToken = userToken1, Word = ""}, "games/" + gameID).Result;

           
            Assert.AreEqual(Forbidden, r3.Status);
        }
        
    }
}
