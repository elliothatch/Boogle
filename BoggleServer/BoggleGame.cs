// Elliot Hatch, u0790511
// Samueld Davidson, u0835059
// November, 2014
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using BB;
using CustomNetworking;

namespace BoggleServer
{
    public class BoggleGame
    {
        //Hold the parent server so that callbacks can be made
        BServer m_server;
        //This game's two player
        public BogglePlayer m_player1 { get; private set; }
        public BogglePlayer m_player2 { get; private set; }
        //Time remaining. Starts at gameLength and ticks down.
        int m_timeRemaining;
        public int m_StartingTime { get; private set; }
        //Board from the supplied BoggleBoard class
        public BoggleBoard m_board { get; private set; }
        //Our Threading Timer which invokes a callback every second when run. 
        Timer m_timer;

        /// <summary>
        /// Constructor that prepares all the member variables for a Boggle game to begin. 
        /// The method startGame will start the game playing.
        /// </summary>
        /// <param name="server">Parent Server</param>
        /// <param name="player1">The first player</param>
        /// <param name="player2">The second player</param>
        /// <param name="gameLength">Length of game</param>
        /// <param name="boardString">16 char starting board length. Random board is empty or invalid.</param>
        public BoggleGame(BServer server, BogglePlayer player1, BogglePlayer player2, int gameLength, string boardString)
        {
            m_server = server;
            m_player1 = player1;
            m_player2 = player2;
            m_timeRemaining = gameLength;
            m_StartingTime = gameLength;
            if (boardString.Length == 16) //Create the board with our without the boardString
                m_board = new BoggleBoard(boardString);
            else
                m_board = new BoggleBoard();
            m_timer = new Timer(timerCallback, null, Timeout.Infinite, 1000); //Initialize the timer but dont count yet.

            player1.game = this;
            player2.game = this;
            player1.opponent = player2;
            player2.opponent = player1;

        }
        /// <summary>
        /// Constructor that prepares all the member variables for a Boggle game to begin. 
        /// The method startGame will start the game playing.
        /// </summary>
        /// <param name="server">Parent Server</param>
        /// <param name="player1">The first player</param>
        /// <param name="player2">The second player</param>
        /// <param name="gameLength">Length of game</param>
        public BoggleGame(BServer server, BogglePlayer player1, BogglePlayer player2, int gameLength)
            : this(server, player1, player2, gameLength, "")
        {
        }
        /// <summary>
        /// Invoke on an independent thread. Begins the timer and listening of the players.
        /// </summary>
        public void startGame()
        {
            ServerPrint("Game Started: " + m_player1.name + " vs. " + m_player2.name + " | Seconds: " + m_timeRemaining + " StartBoard: " + m_board.ToString());

            //Reset the players
            m_player1.ResetGameValues();
            m_player2.ResetGameValues();

            string startMessage = "START " + m_board.ToString() + " " + m_timeRemaining + " ";
            m_player1.stringSocket.BeginSend(startMessage + m_player2.name + "\n", sendCallback, m_player1.name);
            m_player2.stringSocket.BeginSend(startMessage + m_player1.name + "\n", sendCallback, m_player2.name);
            m_timer.Change(1000, 1000);
            //don't receive from player 1, they're already receiving from the server thread
            //m_player1.stringSocket.BeginReceive(receivedPlayerMessage, m_player1);
            m_player2.stringSocket.BeginReceive(receivedPlayerMessage, m_player2);
        }
        /// <summary>
        /// Invoked by the timer every send. Decrements the time remaining and informs the players of the new time.
        /// Also checks if the time is over and ends the game.
        /// </summary>
        /// <param name="state"></param>
        private void timerCallback(object state)
        {
            if (m_timeRemaining > 0) //Time left?
            {
                m_timeRemaining--;
                string timeMessage = "TIME " + m_timeRemaining + "\n"; //Create message.
                m_player1.stringSocket.BeginSend(timeMessage, sendCallback, m_player1.name);
                m_player2.stringSocket.BeginSend(timeMessage, sendCallback, m_player2.name);
            }
            else //Time is up
            {
                m_timer.Dispose(); //Destroy the timer
                IEnumerable<String> UniqueP1Words = m_player1.legalWords.Except<String>(m_player2.legalWords);
                IEnumerable<String> UniqueP2Words = m_player2.legalWords.Except<String>(m_player1.legalWords);
                IEnumerable<String> CommonWords = m_player1.legalWords.Intersect<String>(m_player2.legalWords);
                //Create strings for all the STOP information.
                String uniqueP1WordsString = "";
                foreach (String word in UniqueP1Words) 
                {
                    uniqueP1WordsString += " " + word;
                }
                String uniqueP2WordsString = "";
                foreach (String word in UniqueP2Words)
                {
                    uniqueP2WordsString += " " + word;
                }
                String commonPlayerWords = "";
                foreach (String word in CommonWords)
                {
                    commonPlayerWords += " " + word;
                }
                String illegalP1Words = "";
                foreach (String word in m_player1.illegalWords)
                {
                    illegalP1Words += " " + word;
                }
                String illegalP2Words = "";
                foreach (String word in m_player2.illegalWords)
                {
                    illegalP2Words += " " + word;
                }
                //Construct the STOP message for each player
                String stopP1 = "STOP " + UniqueP1Words.Count() + uniqueP1WordsString + " "
                    + UniqueP2Words.Count() + uniqueP2WordsString + " " +
                    CommonWords.Count() + commonPlayerWords + " " +
                    m_player1.illegalWords.Count + illegalP1Words + " " +
                    m_player2.illegalWords.Count + illegalP2Words + "\n";
                String stopP2 = "STOP " + UniqueP2Words.Count() + uniqueP2WordsString + " " +
                     UniqueP1Words.Count() + uniqueP1WordsString + " " +
                    CommonWords.Count() + commonPlayerWords + " " +
                    m_player2.illegalWords.Count + illegalP2Words + " " +
                     m_player1.illegalWords.Count + illegalP1Words +"\n";

                m_player1.stringSocket.BeginSend(stopP1, sendCallback, m_player1.name);
                m_player2.stringSocket.BeginSend(stopP2, sendCallback, m_player2.name);

                m_player1.stringSocket.Close();
                m_player2.stringSocket.Close();

                m_server.gameCompleted(this); //Inform the server that this game is over.
            }
        }
        
        /// <summary>
        /// Callback for BeginReceive of player string sockets. 
        /// Checks for connection loss and words being sent.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        /// <param name="payload">The boggle player who's message was received.</param>
        public void receivedPlayerMessage(string s, Exception e, object payload)
        {
            BogglePlayer player = (BogglePlayer)payload;
            if(s == null) //Connection is over
            {
                player.opponent.stringSocket.BeginSend("TERMINATED\n", sendCallback, player.opponent.name); //Inform the other player
                player.opponent.stringSocket.Close(); //Close up shop
                m_timer.Dispose();
                m_server.gameCompleted(this); //Inform the parent server.
                ServerPrint("Player " + player.name + " disconnected. Terminating player " + player.opponent.name);
                return; //Kill the thread.
            }
            Console.WriteLine("Received: \"" + s + "\" from" + player.name);

            string[] messageComponents = s.Split(null); //Break up the receieved message.
            if (messageComponents[0].ToUpper() == "WORD") //Check if the players sent a WORD
            {
                String word = messageComponents[1].ToUpper();

                lock (m_board) //lock because we modify player values
                {
                    if (word.Length >= 3 && !player.legalWords.Contains(word) && !player.illegalWords.Contains(word)) //Word validity checking
                    {
                        int wordValue = -1;
                        if (BServer.s_boggleDictionary.Contains(word) && m_board.CanBeFormed(word) ) //Additional Validity checking.
                        {
                            player.legalWords.Add(word);
                            if (word.Length < 5) //Calculate its value is points
                            {
                                wordValue = 1;
                            }
                            else if (word.Length == 5)
                            {
                                wordValue = 2;
                            }
                            else if (word.Length == 6)
                            {
                                wordValue = 3;
                            }
                            else if (word.Length == 7)
                            {
                                wordValue = 5;
                            }
                            else
                            {
                                wordValue = 11;
                            }
                        }
                        else
                        {
                            player.illegalWords.Add(word);
                        }
                        if (player.opponent.legalWords.Contains(word))
                        {
                            player.opponent.score -= wordValue;
                        }
                        else
                        {
                            player.score += wordValue;
                        }
                        //Send new scores
                        player.stringSocket.BeginSend("SCORE " + player.score + " " + player.opponent.score + "\n", sendCallback, player.name);
                        player.opponent.stringSocket.BeginSend("SCORE " + player.opponent.score + " " + player.score + "\n", sendCallback, player.opponent.name);
                    }
                }
            }
            else //The players didnt send a word so its something invalid.
            {
                player.stringSocket.BeginSend("IGNORING " + messageComponents[0] + "\n", sendCallback, player.name);
            }

            player.stringSocket.BeginReceive(receivedPlayerMessage, player);

        }
        /// <summary>
        /// Send callback that prints to the server console that a message was successfully sent or had an error.
        /// </summary>
        private void sendCallback(Exception e, object o)
        {
            if (e != null)
            {
                ServerPrint("Message Send Exception: " + e.Message);
            }
        }
        /// <summary>
        /// Helper method that appends the gameID to each Server message printed from this game.
        /// </summary>
        /// <param name="str"></param>
        private void ServerPrint(string str)
        {
            Console.WriteLine(this.GetHashCode() + ": " + str);
        }


    }
    /// <summary>
    /// A sweet class that I really like. :)
    /// Stores all the data of each player in a game.
    /// </summary>
    public class BogglePlayer
    {
        public StringSocket stringSocket { get; private set; }
        public string name { get; set; }
        public HashSet<String> legalWords { get; private set; }
        public HashSet<String> illegalWords { get; private set; }
        public int score { get; set; }
        public BogglePlayer opponent { get; set; }
        public BoggleGame game { get; set; }
        
        public BogglePlayer(StringSocket _stringSocket)
        {
            stringSocket = _stringSocket;
            name = null;
            legalWords = new HashSet<String>();
            illegalWords = new HashSet<string>();
            score = 0;
            game = null;
        }
        /// <summary>
        /// Resets all the game statistics of a player. 
        /// Not necessary for PS8 but useful if rematch funcationality is added.
        /// </summary>
        public void ResetGameValues()
        {
            score = 0;
            legalWords.Clear();
            illegalWords.Clear();
        }
    }
}
