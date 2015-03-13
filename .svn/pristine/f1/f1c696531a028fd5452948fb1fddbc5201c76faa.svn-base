// Elliot Hatch, u0790511
// Samuel Davidson, u0835059
// December, 2014
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
    class BoggleWebServer
    {
        /// <summary>
        /// The MySQL connection string.
        /// </summary>
        public const string MySQLconnectionString = "server=atr.eng.utah.edu;database=cs3500_ellioth;uid=cs3500_ellioth;password=444892533";

        private TcpListener m_clientListener;
        public BoggleWebServer()
        {
            m_clientListener = new TcpListener(IPAddress.Any, 2500);
            m_clientListener.Start();
        }
        /// <summary>
        /// Blocking method that accepts a new connection as soon as it arrives.
        /// The server then listens to the new connection once established.
        /// </summary>
        public void acceptConnections()
        {
            Socket clientSocket = m_clientListener.AcceptSocket(); //Blocking TCPListener method that waits for a connection.
            StringSocket clientStringSocket = new StringSocket(clientSocket, Encoding.UTF8); //Create the string socket for that connection
            Console.WriteLine("Web Client Connected: " + clientSocket.RemoteEndPoint);
            clientStringSocket.BeginReceive(receivedClientMessage, clientStringSocket); //Start listening
        }

        public void receivedClientMessage(String s, Exception e, object payload)
        {
            if (e != null)
            {
                return;
            }
            if (s == null)
            {
                return;
            }
            Console.WriteLine("Web Server received: " + s);
            StringSocket clientSocket = (StringSocket)payload;
            try
            {
                string[] messageComponents = s.Trim().Split(); //Break the message up into components based on spaces.
                if (messageComponents.Length == 3 && messageComponents[0] == "GET" && messageComponents[2] == "HTTP/1.1")
                {
                    string[] requestURLComponents = messageComponents[1].Split('?');
                    if (requestURLComponents[0] == "/players")
                    {

                        RespondAllPlayersHTML(clientSocket);
                    }
                    else if (requestURLComponents[0] == "/games")
                    {
                        string playerName = "";
                        if (requestURLComponents.Length < 2)
                        {
                            RespondErrorHTML(clientSocket, "Incorrectly formatted player parameter.");
                        }
                        string[] playerParameter = requestURLComponents[1].Split('=');
                        if (playerParameter[0] == "player")
                        {
                            playerName = playerParameter[1];
                            RespondGamesByPlayerHTML(clientSocket, playerName);
                        }
                        else
                        {
                            RespondErrorHTML(clientSocket, "Incorrectly formatted player parameter.");
                        }
                    }
                    else if (requestURLComponents[0] == "/game")
                    {
                        int gameID;
                        if (requestURLComponents.Length < 2)
                        {
                            RespondErrorHTML(clientSocket, "Incorrectly formatted game ID parameter.");
                        }
                        string[] idParameter = requestURLComponents[1].Split('=');
                        if (idParameter[0] == "id")
                        {
                            if (Int32.TryParse(idParameter[1], out gameID))
                            {
                                RespondGameOfIdHTML(clientSocket, gameID);
                            }
                            else
                            {
                                RespondErrorHTML(clientSocket, "Incorrectly formatted game ID parameter");
                            }
                        }
                        else
                        {
                            RespondErrorHTML(clientSocket, "Incorrectly formatted game ID parameter.");
                        }
                    }
                    else if (requestURLComponents[0] == "/main.css")
                    {
                        RespondCSS(clientSocket);
                    }
                    else
                    {
                        RespondErrorHTML(clientSocket, "Incorrectly formatted HTML request.");
                    }
                }
                else
                {
                    RespondErrorHTML(clientSocket, "Incorrectly formatted HTML request.");
                }
            }
            catch (Exception)
            {
                RespondErrorHTML(clientSocket, "Incorrectly formatted HTML request.");
            }
            finally
            {
                clientSocket.Close();
            }
        }
        /// <summary>
        /// Constructs and sends an HTML page containing a list of all recorded boggle players.
        /// </summary>
        private void RespondAllPlayersHTML(StringSocket client)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(MySQLconnectionString))
                {
                    conn.Open();
                    MySqlCommand command = conn.CreateCommand();
                    command.CommandText = @"SELECT name, 
                                            COUNT(CASE WHEN T1.score>(SELECT T2.score FROM Players_Games T2 WHERE T1.gameId=T2.gameId AND T1.playerId!=T2.playerId) THEN 1 END) AS gamesWon,
                                            COUNT(CASE WHEN T1.score<(SELECT T2.score FROM Players_Games T2 WHERE T1.gameId=T2.gameId AND T1.playerId!=T2.playerId) THEN 1 END) AS gamesLost,
                                            COUNT(CASE WHEN T1.score=(SELECT T2.score FROM Players_Games T2 WHERE T1.gameId=T2.gameId AND T1.playerId!=T2.playerId) THEN 1 END) AS gamesTied
                                            FROM Players INNER JOIN Players_Games T1 WHERE Players.id=T1.playerId GROUP BY Players.id ORDER BY gamesWon DESC";
                    // Execute the command and cycle through the DataReader object
                    MySqlDataReader sqlData = command.ExecuteReader();

                    StringBuilder page = new StringBuilder(GetHTMLheader());
                    page.Append("<html><head><link rel=\"stylesheet\" href=\"/main.css\"></head>");
                    page.Append("<body><div class='outerWrap'><div class='topBorder'><span>Boggle Champions 2015</span></div><div class='contentWrap'>");
                    page.Append(getPageHeader());
                    page.Append("<h1>All Players:</h1><table><thead><tr><th>Name</th><th>Games Won</th><th>Games Lost</th><th>Games Tied</th></tr></thead><tbody>");
                    while (sqlData.Read())
                    {

                        page.Append("<tr><td><a href='/games?player=" + sqlData["name"] + "'>" + sqlData["name"] + "</a></td><td>" + sqlData["gamesWon"] + "</td><td>" + sqlData["gamesLost"] + "</td><td>" + sqlData["gamesTied"] + "</td></tr>");
                    }
                    page.Append("</tbody></table>");
                    page.Append("</div></div></body></html>\r\n");
                    client.BeginSend(page.ToString(), (e, p) => { }, null);
                }
            }
            catch(Exception e)
            {
                RespondErrorHTML(client, e.Message);
            }
        }
        /// <summary>
        /// Constructs and sends an HTML page containing all of the game by a specified player.
        /// </summary>
        /// <param name="player">name of the player whose games will be sent.</param>
        private void RespondGamesByPlayerHTML(StringSocket client, string player)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(MySQLconnectionString))
                {
                    conn.Open();
                    MySqlCommand command = conn.CreateCommand();
                    command.CommandText = @"SELECT gameId,
                                           timeCompleted,
                                           (SELECT name FROM Players INNER JOIN Players_Games T1 ON Players.id=T1.playerId WHERE T1.gameId=T2.gameId AND T1.playerId!=T2.playerId) AS 'Opponent',
                                           score,
                                           (SELECT score FROM Players_Games T1 WHERE T1.gameId=T2.gameId AND T1.playerId!=T2.playerId) AS 'opponentScore'
                                           FROM Players INNER JOIN Players_Games T2 ON Players.id=T2.playerId INNER JOIN Games ON Games.id=T2.gameId WHERE name='" + player + "'";
                    // Execute the command and cycle through the DataReader object
                    MySqlDataReader sqlData = command.ExecuteReader();

                    StringBuilder page = new StringBuilder(GetHTMLheader());
                    page.Append("<html><head><link rel=\"stylesheet\" href=\"/main.css\"></head>");
                    page.Append("<body><div class='outerWrap'><div class='topBorder'><span>Boggle Champions 2015</span></div><div class='contentWrap'>");
                    page.Append(getPageHeader());
                    page.Append("<h1>Games by " + player + ":</h1><table><thead><tr><th>Game ID</th><th>Game Time</th><th>Opponent Name</th><th>Score</th><th>Oppenent Score</th></tr></thead><tbody>");

                    while (sqlData.Read())
                    {
                        page.Append("<tr><td><a href='/game?id=" + sqlData["gameId"] + "'>" + sqlData["gameId"] + "</a></td><td>" + sqlData["timeCompleted"] + "</td><td><a href='/games?player=" + sqlData["Opponent"] + "'>" + sqlData["Opponent"] + "</a></td><td>" + sqlData["score"] + "</td><td>" + sqlData["opponentScore"] + "</td></tr>");
                    }
                    page.Append("</tbody></table>");
                    page.Append("</div></div></body></html>\r\n");
                    client.BeginSend(page.ToString(), (e, p) => { }, null);
                }
            }
            catch (Exception e)
            {
                RespondErrorHTML(client, e.Message);
            }
        }
        /// <summary>
        /// Constructs and sends an HTML page containing all the information for the game of chosen ID.
        /// </summary>
        /// <param name="gameID">Game ID of the game to view.</param>
        private void RespondGameOfIdHTML(StringSocket client, int gameID)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(MySQLconnectionString))
                {
                    conn.Open();
                    MySqlCommand command = conn.CreateCommand();

                    command.CommandText = @"SELECT timeCompleted, timeLimit, board FROM Games WHERE id=" + gameID;
                    // Execute the command and cycle through the DataReader object
                    MySqlDataReader sqlData = command.ExecuteReader();
                    sqlData.Read();
                    string timeCompleted = sqlData["timeCompleted"].ToString();
                    string board = sqlData["board"].ToString();
                    string timeLimit = sqlData["timeLimit"].ToString();
                    sqlData.Close();
                    command.CommandText = @"SELECT T2.playerId AS 'id',
                                            (SELECT name FROM Players WHERE Players.id=T2.playerId) AS 'name',
                                            T2.score AS 'score'
                                            FROM Games INNER JOIN Players_Games T2 ON Games.id=T2.gameId INNER JOIN Players ON Players.id=T2.playerId WHERE Games.id=" + gameID;

                    // Execute the command and cycle through the DataReader object
                    sqlData = command.ExecuteReader();
                    sqlData.Read();
                    int id1 = Int32.Parse(sqlData["id"].ToString());
                    string name1 = sqlData["name"].ToString();
                    int score1 = Int32.Parse(sqlData["score"].ToString());
                    sqlData.Read();
                    int id2 = Int32.Parse(sqlData["id"].ToString());
                    string name2 = sqlData["name"].ToString();
                    int score2 = Int32.Parse(sqlData["score"].ToString());
                    sqlData.Close();

                    HashSet<Tuple<string, int>> words1 = new HashSet<Tuple<string, int>>();
                    HashSet<Tuple<string, int>> words2 = new HashSet<Tuple<string, int>>();
                    command.CommandText = @"SELECT T2.playerId AS 'playerId', 
                                                    word AS 'words',
                                                    isValid AS 'isValid'
                                            FROM Games 
                                            INNER JOIN Players_Games T2 ON Games.id=T2.gameId 
                                            INNER JOIN Words ON Words.id=T2.wordsId WHERE Games.id=" + gameID +
                                            " ORDER BY playerId, words";
                    // Execute the command and cycle through the DataReader object
                    sqlData = command.ExecuteReader();
                    while(sqlData.Read())
                    {
                        Tuple<string,int> word = new Tuple<string,int>(sqlData["words"].ToString(), 
                                Int32.Parse(sqlData["isValid"].ToString()));
                        if(Int32.Parse(sqlData["playerId"].ToString()) == id1)
                        {
                            words1.Add(word);
                        }
                        else
                        {
                            words2.Add(word);
                        }
                    }
                    sqlData.Close();
                    StringBuilder page = new StringBuilder(GetHTMLheader());
                    page.Append("<html><head><link rel=\"stylesheet\" href=\"/main.css\"></head>");
                    page.Append("<body><div class='outerWrap'><div class='topBorder'><span>Boggle Champions 2015</span></div><div class='contentWrap'>");
                    page.Append(getPageHeader());
                    page.Append("<div class='gameInfo'><h1 class='gameTitle'><em>"+ name1 + "</em> <span>vs.</span> <em>" + name2 + "</em></h1>");
                    page.Append("<h3>Time Completed: " + timeCompleted + "</h3>");
                    page.Append("<h2>Time Limit: " + timeLimit + "</h2></div>");

                    page.Append("<table class='boggleBoard'>");
                    for(int i = 0; i < 4; i++)
                    {
                        page.Append("<tr>");
                        for(int j = 0; j < 4; j++)
                        {
                            page.Append("<td>" + board[i*4 + j] +"</td>");
                        }
                        page.Append("</tr>");
                    }
                    page.Append("</table>");

                    page.Append("<h1><a href='/games?player=" + name1 + "'>" + name1 + "</a>:</h1>");
                    page.Append("<h3>Score: " + score1 + "</h3>");
                    page.Append("<table>");
                    foreach(Tuple<string,int> word in words1)
                    {
                        if(word.Item2 == 0)
                            page.Append("<tr><td class='invalidWord'>" + word.Item1 + "</td></tr>");
                        else if(words2.Contains(word))
                            page.Append("<tr><td class='commonWord'>" + word.Item1 + "</td></tr>");
                        else
                            page.Append("<tr><td>" + word.Item1 + "</td></tr>");
                    }
                    page.Append("</table>");
                    page.Append("<h1><a href='/games?player=" + name2 + "'>" + name2 + "</a>:</h1>");
                    page.Append("<h3>Score: " + score2 + "</h3>");
                    page.Append("<table>");
                    foreach (Tuple<string, int> word in words2)
                    {
                        if (word.Item2 == 0)
                            page.Append("<tr><td class='invalidWord'>" + word.Item1 + "</td></tr>");
                        else if (words1.Contains(word))
                            page.Append("<tr><td class='commonWord'>" + word.Item1 + "</td></tr>");
                        else
                            page.Append("<tr><td>" + word.Item1 + "</td></tr>");
                    }
                    page.Append("</table>");
                    page.Append("</div></div></body></html>\r\n");
                    client.BeginSend(page.ToString(), (e, p) => { }, null);
                }
            }
            catch (Exception e)
            {
                RespondErrorHTML(client, e.Message);
            }
        }
        /// <summary>
        /// Constructs and sends an HTML page in which an error is displayed.
        /// The nature of the error can be specified and drawn
        /// </summary>
        /// <param name="reason">Reason for which the error occured.</param>
        private void RespondErrorHTML(StringSocket client, string reason)
        {
            StringBuilder page = new StringBuilder(GetHTMLheader());
            page.Append("<html><body>ERROR: " + reason + "</body></html>\r\n");
            client.BeginSend(page.ToString(), (e, p) => { }, null);
        }

        /// <summary>
        /// Constructs and sends an HTML page in which an error is displayed.
        /// The nature of the error can be specified and drawn
        /// </summary>
        /// <param name="reason">Reason for which the error occured.</param>
        private void RespondCSS(StringSocket client)
        {
            StringBuilder page = new StringBuilder(GetHTMLheader());
            page.Append("body { margin: 0; padding: 0; background-color: #FFFFFF; " +
                              " font-family: 'MS Sans Serif', Geneva, sans-serif; }");
            page.Append("header { text-align: center; }");
            page.Append("h1, h2 { font-family: 'Impact', Charcoal, sans-serif; font-weight: lighter; }");
            page.Append("h3, h4, h5 { padding-left: 2em; padding-right: 2em; }");
            page.Append(".outerWrap { position: relative; margin: 0 auto; max-width: 50em; " +
                                      "border: 5px ridge #0058F3; border-top: 0px ridge #00f; " +
                                      "background-color: #F0F0F0; }");
            page.Append(".contentWrap { position: relative; margin: 0 auto; max-width: 40em; " + 
                         " padding-bottom: 5em;}");
            page.Append(".topBorder { height: 35px; color:#FFFFFF;  font-weight: bold;" +
                        "padding-left: 1em; " +
                        "background: #63c0ff; /* Old browsers */" +
                        " background: -moz-linear-gradient(top, #63c0ff 5%, #3991fd 26%, #0058f3 99%); /* FF3.6+ */" +
                        " background: -webkit-gradient(linear, left top, left bottom, color-stop(5%,#63c0ff), color-stop(26%,#3991fd), color-stop(99%,#0058f3)); /* Chrome,Safari4+ */ " +
                        " background: -webkit-linear-gradient(top, #63c0ff 5%,#3991fd 26%,#0058f3 99%); /* Chrome10+,Safari5.1+ */ " +
                        " background: -o-linear-gradient(top, #63c0ff 5%,#3991fd 26%,#0058f3 99%); /* Opera 11.10+ */ " +
                        " background: -ms-linear-gradient(top, #63c0ff 5%,#3991fd 26%,#0058f3 99%); /* IE10+ */ " +
                        " background: linear-gradient(to bottom, #63c0ff 5%,#3991fd 26%,#0058f3 99%); /* W3C */ " +
                        " filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#63c0ff', endColorstr='#0058f3',GradientType=0 ); /* IE6-9 */ }");
            page.Append(".topBorder span { display: inline-block; vertical-align: middle; line-height: 35px;}");
            page.Append(".siteTitle, .siteTitle:hover, .siteTitle:visited " +
                        "{ font-family: 'Impact', Charcoal, sans-serif; font-style: italic; " +
                        "font-size: 64pt; font-weight: bold; color:#000000; text-decoration: none;}");
            page.Append("table { width: 100%; border-collapse: collapse; background-color:#FFF; }");
            page.Append("td, th { border: 1px solid #999; padding: 0.5rem; text-align: left; }");
            page.Append(".boggleBoard { width: 4em; margin: 0 auto; table-layout: fixed;}");
            page.Append(".boggleBoard td { width: 1em; height: 1em; text-align: center; " +
                        "font-family: 'Impact', Charcoal, sans-serif; font-size: 24pt; font-weight: bold; }");
            page.Append(".gameTitle { font-size: 48pt; padding-left: 0em; padding-right: 0em; }");
            page.Append(".gameTitle span { font-size: 24pt; padding-left: 0.25em; }");
            page.Append(".gameInfo { text-align: center; }");
            page.Append(".invalidWord { background-color: #FFA6A6; }");
            page.Append(".commonWord { background-color: #FFFFA6; }");
            page.Append("\r\n");
            client.BeginSend(page.ToString(), (e, p) => { }, null);
        }

        /// <summary>
        /// Returns the header used for construction of HTML.
        /// Will be used any time an HTML string is constructed.
        /// </summary>
        /// <returns>HTML header.</returns>
        private string GetHTMLheader()
        {
            return "HTTP/1.1 200 OK\r\nConnection: close\r\nContent-Type: text/html; charset=UTF-8\r\n\r\n";
        }

        private string getPageHeader()
        {
            return "<header><div><a class='siteTitle' href='/players'>Boggle</a></div></header>";
        }
    }
}
