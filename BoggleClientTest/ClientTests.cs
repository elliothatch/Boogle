//ClientTests.cs - Elliot Hatch and Samuel Davidson - December 2014
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading;
using BoggleClient;
using BoggleServer;

namespace BoggleClientTest
{
    [TestClass]
    public class ClientTests
    {
        /// <summary>
        /// Initialize the dictionary
        /// </summary>
        [TestInitialize()]
        public void Initialize()
        {
            //Load our dictionary into a HashSet
            BServer.s_boggleDictionary = new HashSet<string>(System.IO.File.ReadAllLines("dictionary.txt"));
        }

        /// <summary>
        /// Test connection, timer, and game finished callbacks.
        /// </summary>
        [TestMethod]
        public void SimpleGameTest()
        {
            String startingBoard = "ABCDEFGHIJKLMNOP";
            int gameTime = 5;
            int t1 = gameTime - 1;
            int t2 = gameTime - 1;
            ManualResetEvent mre1 = new ManualResetEvent(false);
            ManualResetEvent mre2 = new ManualResetEvent(false);

            BServer server = new BServer(gameTime, startingBoard);

            BoogleClient client1 = new BoogleClient();
            client1.connectingCallback = (e) => Assert.AreEqual(e, null);
            client1.gameStartCallback =
                (b, t, n) => { Assert.AreEqual(b, startingBoard);
                               Assert.AreEqual(t, gameTime);
                               Assert.AreEqual(n, "sam"); };
            client1.timerCallback = (t) => { Assert.AreEqual(t, t1); t1--; };
            client1.gameFinishedCallback = 
                (lw, olw, cw, mil, oil) =>
                                      {   Assert.AreEqual(lw.Count, 0);
                                          Assert.AreEqual(olw.Count, 0);
                                          Assert.AreEqual(cw.Count, 0);
                                          Assert.AreEqual(mil.Count, 0);
                                          Assert.AreEqual(oil.Count, 0);
                                          mre1.Set(); };

            BoogleClient client2 = new BoogleClient();
            client2.connectingCallback = (e) => Assert.AreEqual(e, null);
            client2.gameStartCallback =
                (b, t, n) =>
                {
                    Assert.AreEqual(b, startingBoard);
                    Assert.AreEqual(t, gameTime);
                    Assert.AreEqual(n, "elliot");
                };
            client2.timerCallback = (t) => { Assert.AreEqual(t, t2); t2--; };
            client2.gameFinishedCallback =
                (lw, olw, cw, mil, oil) =>
                {
                    Assert.AreEqual(lw.Count, 0);
                    Assert.AreEqual(olw.Count, 0);
                    Assert.AreEqual(cw.Count, 0);
                    Assert.AreEqual(mil.Count, 0);
                    Assert.AreEqual(oil.Count, 0);
                    mre2.Set(); 
                };

            client1.Connect("localhost", "elliot");
            server.acceptConnections();
            client2.Connect("localhost", "sam");
            server.acceptConnections();

            Assert.AreEqual(true, mre1.WaitOne(8000), "Timed out waiting 1"); //Give it 8 seconds to play a 5 second game
            Assert.AreEqual(true, mre2.WaitOne(8000), "Timed out waiting 2"); // 

        }

        /// <summary>
        /// test SendWord, score and gameFinished callbacks
        /// </summary>
        [TestMethod]
        public void SimpleScoreTest()
        {
            String startingBoard = "BANAESANIJKEHIBY";
            int gameTime = 5;
            int[] s1 = { 1, 1, 0, 0, 0 };
            int sc1 = 0; //s1 index counter
            int[] s2 = { 0, 5, 5, 4, 3 };
            int sc2 = 0;

            ManualResetEvent mre1 = new ManualResetEvent(false);
            ManualResetEvent mre2 = new ManualResetEvent(false);

            BServer server = new BServer(gameTime, startingBoard);

            BoogleClient client1 = new BoogleClient();
            client1.connectingCallback = (e) => Assert.AreEqual(e, null);
            client1.scoreCallback = (m, o) =>
                                            {
                                                Assert.AreEqual(m, s1[sc1]);
                                                Assert.AreEqual(o, s2[sc1]); sc1++;
                                            };
            client1.gameFinishedCallback =
                (lw, olw, cw, mil, oil) =>
                {
                    Assert.IsTrue(lw.SetEquals(new string[] {}));
                    Assert.IsTrue(olw.SetEquals(new string[] { "BANANAS" }));
                    Assert.IsTrue(cw.SetEquals(new string[] { "BYE" }));
                    Assert.IsTrue(mil.SetEquals(new string[] { }));
                    Assert.IsTrue(oil.SetEquals(new string[] { "CUD", "JKEN" }));
                    mre1.Set();
                };

            BoogleClient client2 = new BoogleClient();
            client2.connectingCallback = (e) => Assert.AreEqual(e, null);
            client2.scoreCallback = (m, o) =>
            {
                Assert.AreEqual(m, s2[sc2]);
                Assert.AreEqual(o, s1[sc2]); sc2++;
            };
            client2.gameFinishedCallback =
                (lw, olw, cw, mil, oil) =>
                {
                    Assert.IsTrue(lw.SetEquals(new string[] { "BANANAS" }));
                    Assert.IsTrue(olw.SetEquals(new string[] { }));
                    Assert.IsTrue(cw.SetEquals(new string[] { "BYE" }));
                    Assert.IsTrue(mil.SetEquals(new string[] { "CUD", "JKEN" }));
                    Assert.IsTrue(oil.SetEquals(new string[] { }));
                    mre2.Set();
                };

            client1.Connect("localhost", "elliot");
            server.acceptConnections();
            client2.Connect("localhost", "sam");
            server.acceptConnections();

            client1.SendWord("bye");
            Thread.Sleep(100);
            client1.SendWord("bye");
            Thread.Sleep(100);
            client1.SendWord("hi");
            Thread.Sleep(100);
            client2.SendWord("bananas");
            Thread.Sleep(100);
            client2.SendWord("bye");
            Thread.Sleep(100);
            client2.SendWord("cud");
            Thread.Sleep(100);
            client2.SendWord("jken");


            Assert.AreEqual(true, mre1.WaitOne(8000), "Timed out waiting 1"); //Give it 8 seconds to play a 5 second game
            Assert.AreEqual(true, mre2.WaitOne(8000), "Timed out waiting 2"); // 

        }

        /// <summary>
        /// test disconnect callbacks and reconnect after disconnect
        /// </summary>
        [TestMethod]
        public void DisconnectTest()
        {
            int gameTime = 5;
            int dc1 = 0;
            int dc2 = 0;
            ManualResetEvent mre1 = new ManualResetEvent(false);
            ManualResetEvent mre2 = new ManualResetEvent(false);

            BServer server = new BServer(gameTime);

            BoogleClient client1 = new BoogleClient();
            client1.connectingCallback = (e) => Assert.AreEqual(e, null);
            client1.disconnectedCallback = (s) => { dc1++; };
            client1.gameFinishedCallback = (lw, olw, cw, mil, oil) => mre1.Set();

            BoogleClient client2 = new BoogleClient();
            client2.connectingCallback = (e) => Assert.AreEqual(e, null);
            client2.disconnectedCallback = (s) => { dc2++; };
            client2.gameFinishedCallback = (lw, olw, cw, mil, oil) => mre2.Set();

            client1.Connect("localhost", "elliot");
            server.acceptConnections();

            Thread.Sleep(100);
            client1.Disconnect();

            Thread.Sleep(100);
            client1.Connect("localhost", "elliot");
            server.acceptConnections();

            client2.Connect("localhost", "sam");
            server.acceptConnections();

            Thread.Sleep(100);
            client1.Disconnect();

            mre1.Reset();
            mre2.Reset();

            Thread.Sleep(100);
            client1.Connect("localhost", "elliot");
            server.acceptConnections();
            client2.Connect("localhost", "sam");
            server.acceptConnections();

            Assert.AreEqual(true, mre1.WaitOne(8000), "Timed out waiting 1"); //Give it 8 seconds to play a 5 second game
            Assert.AreEqual(true, mre2.WaitOne(8000), "Timed out waiting 2"); // 

            Thread.Sleep(100);
            Assert.AreEqual(dc1, 1);
            Assert.AreEqual(dc2, 2);

        }
    }
}
