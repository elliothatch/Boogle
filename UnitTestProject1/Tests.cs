// Elliot Hatch, u0790511
// Samuel Davidson, u0835059
// November, 2014
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BoggleServer;
using System.Net.Sockets;
using System.Threading;
using CustomNetworking;
using System.Text;
using System.Collections.Generic;

//TESTS ACHIEVE 92% CODE COVERAGE
//MOST OF THE NON-COVERED CODE IS IN PROGRAM.CS AND BOGGLEBOARD.CS
namespace BoggleServerTest
{
    [TestClass]
    public class Tests
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
        /// Tests that the server can estabilsh and complete a boggle game between two clients
        /// The clients do not send words to the server
        /// </summary>
        [TestMethod]
        public void SimpleGameToCompletion()
        {
            String startingBoard = "ABCDEFGHIJKLMNOP";
            BServer server = new BServer(5, startingBoard);
            TestClient clientOne = new TestClient("localhost");
            server.acceptConnections();
            TestClient clientTwo = new TestClient("localhost");
            server.acceptConnections();
            
            clientOne.socket.BeginSend("Play Sam\n", SGTCSendCallback, "Client One");
            clientTwo.socket.BeginSend("Play Elliot\n", SGTCSendCallback, "Client Two");
            clientOne.socket.BeginReceive(SGTCReceiveCallback, "START ABCDEFGHIJKLMNOP 5 Elliot");
            clientTwo.socket.BeginReceive(SGTCReceiveCallback, "START ABCDEFGHIJKLMNOP 5 Sam");
            clientOne.socket.BeginReceive(SGTCReceiveCallback, "TIME 4");
            clientTwo.socket.BeginReceive(SGTCReceiveCallback, "TIME 4");
            clientOne.socket.BeginReceive(SGTCReceiveCallback, "TIME 3");
            clientTwo.socket.BeginReceive(SGTCReceiveCallback, "TIME 3");
            clientOne.socket.BeginReceive(SGTCReceiveCallback, "TIME 2");
            clientTwo.socket.BeginReceive(SGTCReceiveCallback, "TIME 2");
            clientOne.socket.BeginReceive(SGTCReceiveCallback, "TIME 1");
            clientTwo.socket.BeginReceive(SGTCReceiveCallback, "TIME 1");
            clientOne.socket.BeginReceive(SGTCReceiveCallback, "TIME 0");
            clientTwo.socket.BeginReceive(SGTCReceiveCallback, "TIME 0"); 
            
            ManualResetEvent mre1 = new ManualResetEvent(false);
            ManualResetEvent mre2 = new ManualResetEvent(false);
            clientOne.socket.BeginReceive((s, e, p) => { SGTCReceiveCallback(s, e, p); mre1.Set(); }, "STOP 0 0 0 0 0");
            clientTwo.socket.BeginReceive((s, e, p) => { SGTCReceiveCallback(s, e, p); mre2.Set(); }, "STOP 0 0 0 0 0");

           
            Assert.AreEqual(true, mre1.WaitOne(10000), "Timed out waiting 1"); //Give it 10s to establish a connection at play a 5s game.
            Assert.AreEqual(true, mre2.WaitOne(10000), "Timed out waiting 2"); // 
            
            
        }

        /// <summary>
        /// Tests the word and scoring capabilities of the server
        /// </summary>
        [TestMethod]
        public void SimpleGameToCompletion2()
        {
            String startingBoard = "BANAESANIJKEHIBY";
            BServer server = new BServer(5, startingBoard);
            TestClient clientOne = new TestClient("localhost");
            server.acceptConnections();
            TestClient clientTwo = new TestClient("localhost");
            server.acceptConnections();

            clientOne.socket.BeginSend("Play Sam\n", SGTCSendCallback, "Client One");
            clientTwo.socket.BeginSend("Play Elliot\n", SGTCSendCallback, "Client Two");
            clientOne.socket.BeginReceive(SGTCReceiveCallback, "START BANAESANIJKEHIBY 5 Elliot");
            clientTwo.socket.BeginReceive(SGTCReceiveCallback, "START BANAESANIJKEHIBY 5 Sam");
            clientOne.socket.BeginReceive(SGTCReceiveCallback, "TIME 4");
            clientTwo.socket.BeginReceive(SGTCReceiveCallback, "TIME 4");
            Thread.Sleep(1500);
            clientOne.socket.BeginSend("Word bye\n", SGTCSendCallback, "Client One"); //add score
            clientOne.socket.BeginReceive(SGTCReceiveCallback, "SCORE 1 0");
            clientTwo.socket.BeginReceive(SGTCReceiveCallback, "SCORE 0 1");
            clientOne.socket.BeginSend("Word bye\n", SGTCSendCallback, "Client One"); //duplicate doesn't change score
            clientOne.socket.BeginSend("Word hi\n", SGTCSendCallback, "Client One"); //doesn't trigger score change
            clientTwo.socket.BeginSend("Word bananas\n", SGTCSendCallback, "Client Two"); //add score
            clientOne.socket.BeginReceive(SGTCReceiveCallback, "SCORE 1 5");
            clientTwo.socket.BeginReceive(SGTCReceiveCallback, "SCORE 5 1");
            clientTwo.socket.BeginSend("Word bye\n", SGTCSendCallback, "Client Two"); //subtract from opponent
            clientOne.socket.BeginReceive(SGTCReceiveCallback, "SCORE 0 5");
            clientTwo.socket.BeginReceive(SGTCReceiveCallback, "SCORE 5 0");
            clientTwo.socket.BeginSend("Word cud\n", SGTCSendCallback, "Client Two"); //in dictionary, not board
            clientOne.socket.BeginReceive(SGTCReceiveCallback, "SCORE 0 4");
            clientTwo.socket.BeginReceive(SGTCReceiveCallback, "SCORE 4 0");
            clientTwo.socket.BeginSend("Word jken\n", SGTCSendCallback, "Client Two"); //in board, not dictionary
            clientOne.socket.BeginReceive(SGTCReceiveCallback, "SCORE 0 3");
            clientTwo.socket.BeginReceive(SGTCReceiveCallback, "SCORE 3 0");

            clientOne.socket.BeginReceive(SGTCReceiveCallback, "TIME 3");
            clientTwo.socket.BeginReceive(SGTCReceiveCallback, "TIME 3");
            clientOne.socket.BeginReceive(SGTCReceiveCallback, "TIME 2");
            clientTwo.socket.BeginReceive(SGTCReceiveCallback, "TIME 2");
            clientOne.socket.BeginReceive(SGTCReceiveCallback, "TIME 1");
            clientTwo.socket.BeginReceive(SGTCReceiveCallback, "TIME 1");
            clientOne.socket.BeginReceive(SGTCReceiveCallback, "TIME 0");
            clientTwo.socket.BeginReceive(SGTCReceiveCallback, "TIME 0");

            ManualResetEvent mre1 = new ManualResetEvent(false);
            ManualResetEvent mre2 = new ManualResetEvent(false);
            clientOne.socket.BeginReceive((s, e, p) => { SGTCReceiveCallback(s, e, p); mre1.Set(); },
                "STOP 0 1 BANANAS 1 BYE 0 2 CUD JKEN");
            clientTwo.socket.BeginReceive((s, e, p) => { SGTCReceiveCallback(s, e, p); mre2.Set(); },
                "STOP 1 BANANAS 0 1 BYE 2 CUD JKEN 0");


            Assert.AreEqual(true, mre1.WaitOne(10000), "Timed out waiting 1"); //Give it 10s to establish a connection and play a 5s game.
            Assert.AreEqual(true, mre2.WaitOne(10000), "Timed out waiting 2"); // 


        }

        /// <summary>
        /// Tests that the server can handle disconnecting clients and non-valid messages sent by a client
        /// </summary>
        [TestMethod]
        public void DisconnectTest()
        {
            BServer server = new BServer(5);
            TestClient clientOne = new TestClient("localhost");
            server.acceptConnections();
            TestClient clientTwo = new TestClient("localhost");
            server.acceptConnections();
            TestClient clientThree = new TestClient("localhost");
            server.acceptConnections();

            clientThree.socket.BeginSend("HELLO SERVER\n", SGTCSendCallback, "Client Three");
            clientThree.socket.BeginReceive(SGTCReceiveCallback, "IGNORING HELLO");

            clientThree.socket.Close();

            clientOne.socket.BeginSend("Play    \t\rSam\n", SGTCSendCallback, "Client One");
            clientTwo.socket.BeginSend("Play Elliot\n", SGTCSendCallback, "Client Two");
            clientOne.socket.BeginReceive((s, e, p) => { }, null);
            clientTwo.socket.BeginReceive((s, e, p) => { }, null);
            clientOne.socket.BeginReceive((s, e, p) => { SGTCReceiveCallback(s, e, p); 
                clientOne.socket.BeginSend("HIYA SERVER\n", SGTCSendCallback, "Client One"); }, "TIME 4");
            clientOne.socket.BeginReceive(SGTCReceiveCallback, "IGNORING HIYA");
            clientTwo.socket.BeginReceive(SGTCReceiveCallback, "TIME 4");
            clientOne.socket.BeginReceive(SGTCReceiveCallback, "TIME 3");
            clientTwo.socket.BeginReceive((s, e, p) => { clientTwo.socket.Close(); }, "TIME 3");
            ManualResetEvent mre1 = new ManualResetEvent(false);
            clientOne.socket.BeginReceive((s, e, p) => { SGTCReceiveCallback(s, e, p); mre1.Set(); }, "TERMINATED");

            Assert.AreEqual(true, mre1.WaitOne(5000), "Timed out waiting 1"); //Give it 10s to establish a connection at play a 5s game.


        }

        public void SGTCSendCallback(Exception e, object payload)
        {
            Assert.AreEqual(null, e);
        }
        public void SGTCReceiveCallback(String s, Exception e, object payload)
        {
            Assert.AreEqual(null, e);

            Assert.AreEqual((String)payload, s);
        }
    }
    class TestClient
    {

        public StringSocket socket;

        public TestClient(String ip)
        {
            TcpClient client = new TcpClient(ip, 2000);
            socket = new StringSocket(client.Client, Encoding.UTF8);

        }

    }
}
