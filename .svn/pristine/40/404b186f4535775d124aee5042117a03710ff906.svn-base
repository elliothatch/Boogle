// Elliot Hatch, u0790511
// Samueld Davidson, u0835059
// November, 2014
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using CustomNetworking;

namespace TestingClient
{
    /// <summary>
    /// Console application used for testing the Boggle Server. 
    /// Operates similar to telnet where you can type commands and it sends them to the server.
    /// Messages received by the server are printed out. 
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            //Build a client
            TestClient client = new TestClient("localhost");
            client.sendMessage("play " + new Random().Next(999) + "\n"); //Automatically play with a random name
            while (true)
            {
                String message = Console.ReadLine(); //Read and send any typed messages
                client.sendMessage(message + "\n");
            }

            
        }

    }
    /// <summary>
    /// Handles all the networking for the main thread. Sends messages and writes lines of received messages.
    /// </summary>
    class TestClient {
        StringSocket server;
        int messageCount;

        public TestClient(String ip) {
            //Networking setup with the given ip
            TcpClient client = new TcpClient(ip, 2000);
            server = new StringSocket(client.Client, Encoding.UTF8);

            server.BeginReceive(receiveCallback, messageCount);

        }
        /// <summary>
        /// Calls BeginSend on the internal StringSocketServer.
        /// Originally had an a writeline in it so this method was more justified.
        /// </summary>
        /// <param name="message"></param>
        public void sendMessage(string message) {
            server.BeginSend(message, null, null);
        }
        /// <summary>
        /// Handles closing connections and telling the user what the server said.
        /// Increments the message count.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="e"></param>
        /// <param name="payload"></param>
        private void receiveCallback(String message, Exception e, object payload)
        {
            if (message == null)
            {
                //server.Close();
                Console.WriteLine("Connection Closed");
                return;
            }
            messageCount++;
            //if(message.Split(' ')[0].ToUpper() == "STOP") 
                Console.WriteLine("Server:" + messageCount +": "+ message);
                //if (message.Split(' ')[0].ToUpper() == "STOP")
                //    return;

            server.BeginReceive(receiveCallback, messageCount);
        }


    }
}
