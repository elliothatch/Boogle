// Elliot Hatch, u0790511
// Samuel Davidson, u0835059
// December, 2014
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using CustomNetworking;

namespace BoggleClient
{

    public class BoogleClient
    {
        private StringSocket server;
        private TcpClient client;
        private string username;
        private int messageCount;
        /// <summary>
        /// Params: (Exception connectionException)
        /// Callback for when the client has completed a connecting action. If the connect was successful
        /// the exception is null, otherwise it is the exception raised when the client couldn't connect.
        /// </summary>
        public Action<Exception> connectingCallback;
        /// <summary>
        /// Params: (string board, int gameTime, string opponentName)
        /// Callback for when the server begins a game.
        /// </summary>
        public Action<string, int, string> gameStartCallback;
        /// <summary>
        /// Params: (int time)
        /// Callback for when the server's timer has updated.
        /// </summary>
        public Action<int> timerCallback;
        /// <summary>
        /// Params: (int myScore, opponentScore)
        /// Callback for when the game score changes.
        /// </summary>
        public Action<int, int> scoreCallback;
        /// <summary>
        /// Params: (ISet myLegalWords, ISet opponentLegalWords,
        /// ISet commonLegalWords, ISet myIllegalWords, ISet opponentIllegalWords)
        /// Callback for when the game has completed.
        /// </summary>
        public Action<ISet<string>, ISet<string>, ISet<string>, ISet<string>, ISet<string>> gameFinishedCallback;
        /// <summary>
        /// Params: (string disconnectMessage)
        /// Callback for when the client is disconnected.
        /// </summary>
        public Action<string> disconnectedCallback;

        /// <summary>
        /// Constructs a boggle client
        /// </summary>
        public BoogleClient()
        {

        }

        /// <summary>
        /// Asynchronously connects to the target host with the given username
        /// If the client is already connected to a server, disconnects from that server before connecting to the new server
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="_username"></param>
        public void Connect(string ipAddress, string _username)
        {
            if (client != null)
                Disconnect();

            try
            {
                client = new TcpClient();
                username = _username;
                client.BeginConnect(ipAddress, 2000, OnConnect, client);
            }
            catch(Exception e)
            {
                if (connectingCallback != null)
                    connectingCallback(e);
            }
        }

        /// <summary>
        /// Callback function when the connect is complete
        /// </summary>
        /// <param name="asyncResult"></param>
        private void OnConnect(IAsyncResult asyncResult)
        {
            //check that the client in the callback is the same as the member variable
            //if they aren't the same (or the member var is null) then the client has been requested to
            //cancel connecting or to connect to a different host, so do nothing
            TcpClient connectingClient = (TcpClient)asyncResult.AsyncState;
            if (client == connectingClient)
            {
                try
                {
                    client.EndConnect(asyncResult);
                    server = new StringSocket(client.Client, Encoding.UTF8);

                    server.BeginSend("PLAY " + username + "\n",
                        (e, p) => { if (connectingCallback != null) connectingCallback(e); }, null);

                    server.BeginReceive(ReceiveCallback, messageCount);
                }
                catch (Exception e)
                {
                    if (connectingCallback != null)
                        connectingCallback(e);
                }
            }
        }

        /// <summary>
        /// Callback for when the server sends a message
        /// </summary>
        /// <param name="message"></param>
        /// <param name="e"></param>
        /// <param name="payload"></param>
        private void ReceiveCallback(string message, Exception e, object payload)
        {
            if (message == null)
            {
                //the server ended the connection
                server = null;
                Disconnect();
                if (disconnectedCallback != null)
                    disconnectedCallback("Lost connection to server.");
                return;
            }
            messageCount++;
            string[] messages = message.Split(null);
            if(messages[0] == "START")
            {
                if(gameStartCallback != null) 
                    gameStartCallback(messages[1], Int32.Parse(messages[2]), messages[3]);
            }
            else if(messages[0] == "TIME")
            {
                if (timerCallback != null) 
                    timerCallback(Int32.Parse(messages[1]));
            }
            else if(messages[0] == "SCORE")
            {
                if (scoreCallback != null) 
                    scoreCallback(Int32.Parse(messages[1]), Int32.Parse(messages[2]));
            }
            else if(messages[0] == "STOP")
            {
                if (gameFinishedCallback != null)
                {
                    //parse the STOP message
                    ISet<string>[] wordSets = new ISet<string>[5];
                    int index = 1;
                    for (int i = 0; i < 5; i++)
                    {
                        wordSets[i] = new HashSet<string>();
                        int wordCount;
                        Int32.TryParse(messages[index], out wordCount);
                        index++;

                        int wordIndex = 0;
                        while (wordIndex < wordCount && index < messages.Length)
                        {
                            wordSets[i].Add(messages[index]);
                            index++;
                            wordIndex++;
                        }
                    }
                    gameFinishedCallback(wordSets[0], wordSets[1], wordSets[2], wordSets[3], wordSets[4]);
                }
            }
            else if(messages[0] == "TERMINATED")
            {
                server = null;
                Disconnect();
                if (disconnectedCallback != null)
                    disconnectedCallback("Lost connection to player.");
            }

            if(server != null)
                server.BeginReceive(ReceiveCallback, messageCount);

        }

        /// <summary>
        /// Send a word to the server to be scored in the boggle game
        /// If not connected to a server do nothing
        /// </summary>
        /// <param name="word"></param>
        public void SendWord(String word)
        {
            if(server != null && word.Length > 2)
                server.BeginSend("WORD " + word + "\n",(e,p) => {},null);
        }

        /// <summary>
        /// Disconnect from the server
        /// If not connected to a server do nothing
        /// </summary>
        public void Disconnect()
        {
            if(server != null)
            {
                server.Close();
                server = null;
            }
            if(client != null)
            {
                client.Close();
                client = null;
            }
        }
    }
    

}
