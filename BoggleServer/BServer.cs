// Elliot Hatch, u0790511
// Samuel Davidson, u0835059
// November, 2014
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using CustomNetworking;
using MySql.Data.MySqlClient;

namespace BoggleServer
{
    public class BServer
    {
        //Member variables holding the passed argument from Main
        private int m_gameLength; 
        private string m_boardString;
        //This servers listener for new connections
        private TcpListener m_clientListener;
        //List of active games, may be useful if the server needs to shut down all active games
        private List<BoggleGame> m_activeGames;
        //The waiting player who typed 'play' but has yet to be matched
        private BogglePlayer m_waitingPlayer;
        //Static dictionary for looking up words
        public static HashSet<string> s_boggleDictionary;
        /// <summary>
        /// The MySQL connection string.
        /// </summary>
        public const string MySQLconnectionString = "server=atr.eng.utah.edu;database=cs3500_ellioth;uid=cs3500_ellioth;password=444892533;Allow User Variables=True";
        /// <summary>
        /// Boggle Server constructor. Takes the length in seconds of games and the starting board.
        /// Sets up all the networking and prepares the server to accept connections.
        /// </summary>
        /// <param name="gameLength">Length of game in seconds</param>
        /// <param name="boardString">16 character starting board string</param>
        public BServer(int gameLength, string boardString)
        {
            m_gameLength = gameLength;
            m_boardString = boardString;
            m_clientListener = new TcpListener(IPAddress.Any, 2000);
            m_clientListener.Start();
            m_activeGames = new List<BoggleGame>();
        }
        /// <summary>
        /// Boggle Server Constructor. Takes in the length in seconds and starts the game with a random starting board.
        /// </summary>
        /// <param name="gameLength"></param>
        public BServer(int gameLength)
            : this(gameLength, ""){}
        /// <summary>
        /// Blocking method that accepts a new connection as soon as it arrives.
        /// The server then listens to the new connection once established.
        /// </summary>
        public void acceptConnections()
        {
            Socket playerSocket = m_clientListener.AcceptSocket(); //Blocking TCPListener method that waits for a connection.
            StringSocket playerStringSocket = new StringSocket(playerSocket, Encoding.UTF8); //Create the string socket for that connection
            Console.WriteLine("Player Connected: " + playerSocket.RemoteEndPoint);
            BogglePlayer player = new BogglePlayer(playerStringSocket);
            playerStringSocket.BeginReceive(receivedPlayerMessage, player); //Start listening
        }
        /// <summary>
        /// Function called by BoggleGames which informs the server that the game has ended.
        /// Removes the game from the list of active games. Useful for the future.
        /// </summary>
        /// <param name="game"></param>
        public void gameCompleted(BoggleGame game)
        {
            lock (m_activeGames)
            {
                m_activeGames.Remove(game);
            }

            BogglePlayer playerOne = game.m_player1;
            BogglePlayer playerTwo = game.m_player2;
            String board = game.m_board.ToString();
            int gameLength = game.m_StartingTime;

            try
            {
                using (MySqlConnection conn = new MySqlConnection(MySQLconnectionString))
                {
                    StringBuilder ourSqlQuery = new StringBuilder();
                    ourSqlQuery.Append("INSERT INTO Players (name) VALUES ('" + playerOne.name + 
                                       "') ON DUPLICATE KEY UPDATE name = '" + playerOne.name + "';");
                    ourSqlQuery.Append("INSERT INTO Players (name) VALUES ('" + playerTwo.name +
                                       "') ON DUPLICATE KEY UPDATE name = '" + playerTwo.name + "';");
                    ourSqlQuery.Append("INSERT INTO Games (timeLimit,board) VALUES (" + gameLength + ",'" + board +
                                       "');");
                    ourSqlQuery.Append("SET @gameID:=LAST_INSERT_ID();");
                    ourSqlQuery.Append(
                        "INSERT INTO Players_Games (playerId, gameId, score) VALUES ((Select Players.id FROM Players WHERE name='" +
                        playerOne.name + "'),@gameID, " + playerOne.score + ");");
                    ourSqlQuery.Append("SET @firstWords:=LAST_INSERT_ID();");
                    ourSqlQuery.Append(
                        "INSERT INTO Players_Games (playerId, gameId, score) VALUES ((Select Players.id FROM Players WHERE name='" +
                        playerTwo.name + "'),@gameID, " + playerTwo.score + ");");
                    ourSqlQuery.Append("SET @secondWords:=LAST_INSERT_ID();");
                    foreach (String legalWords in playerOne.legalWords)
                    {
                        ourSqlQuery.Append("INSERT INTO Words (id, word, isValid) VALUES (@firstWords, '"+legalWords+"', 1);");
                    }
                    foreach (String illegalWords in playerOne.illegalWords)
                    {
                        ourSqlQuery.Append("INSERT INTO Words (id, word, isValid) VALUES (@firstWords, '" + illegalWords + "', 0);");
                    }
                    foreach (String legalWords in playerTwo.legalWords)
                    {
                        ourSqlQuery.Append("INSERT INTO Words (id, word, isValid) VALUES (@secondWords, '" + legalWords + "', 1);");
                    }
                    foreach (String illegalWords in playerTwo.illegalWords)
                    {
                        ourSqlQuery.Append("INSERT INTO Words (id, word, isValid) VALUES (@secondWords, '" + illegalWords + "', 0);");
                    }
                    conn.Open();
                    MySqlCommand command = conn.CreateCommand();
                    command.CommandText = ourSqlQuery.ToString();
                    command.ExecuteReader();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("MySQL Write Error: " + e.Message);
            }
        }
        /// <summary>
        /// Callback for received messages from connections that have not been placed into a game.
        /// Basically checks for "PLAY @" where @ is a player name, and places that player into the queue or gets them into a game.
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        /// <param name="payload"></param>
        public void receivedPlayerMessage(String s, Exception e, object payload)
        {
            BogglePlayer player = (BogglePlayer)payload;
            if (player.game != null)
            {
                //the first message the waiting player sends will be intercepted by this thread (in case the player disconnects)
                //so we forward the message to the game the player is in
                player.game.receivedPlayerMessage(s, e, payload);
                return;
            }
            if (s == null) //Connection was lost.
            {
                if (m_waitingPlayer != null && player.stringSocket == m_waitingPlayer.stringSocket)
                    m_waitingPlayer = null;
                Console.WriteLine("Player Disconnected");
                return;
            }
            string[] messageComponents = s.Split(); //Break the message up into components based on spaces.
            if (messageComponents[0].ToUpper() == "PLAY") //They want to play
            {
                player.name = messageComponents[1]; //set the player's name.
                lock (m_activeGames)
                {
                    if (m_waitingPlayer == null) //Needs to wait
                    {
                        m_waitingPlayer = player;
                        Console.WriteLine(m_waitingPlayer.name + " is ready to play."); //Inform that a player is waiting
                        player.stringSocket.BeginReceive(receivedPlayerMessage, player);
                    }
                    else //Pair the two up and start a game
                    {
                        BoggleGame game;
                        if (m_boardString.Length == 0) //Two initializers on whether or not the boardstring was supplied.
                            game = new BoggleGame(this, m_waitingPlayer, player, m_gameLength);
                        else
                            game = new BoggleGame(this, m_waitingPlayer, player, m_gameLength, m_boardString);

                        m_activeGames.Add(game);
                        Task.Factory.StartNew(() => { game.startGame(); }); //Start a game in its own thread.

                        m_waitingPlayer = null;
                    }
                }
            }
            else
            {
                player.stringSocket.BeginSend("IGNORING " + messageComponents[0] + "\n", (c, p) => { Console.WriteLine("IGNORING " + messageComponents[0] + "\n"); }, null); //The message was invalid.
                player.stringSocket.BeginReceive(receivedPlayerMessage, player);
            }

        }
    }
}