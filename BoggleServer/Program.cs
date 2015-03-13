// Elliot Hatch, u0790511
// Samuel Davidson, u0835059
// November, 2014
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace BoggleServer
{
    class Program
    {

        static void Main(string[] args)
        {
            if(args.Length < 2 || args.Length > 3) //Main argument checking
            {
                Console.WriteLine("Incorrect arguments - gameTime dictionaryPath [startingBoard]");
                return;
            }

            int gameLength;
            if(!Int32.TryParse(args[0], out gameLength)) //More argument checking
            {
                Console.WriteLine("Incorrect arguments - invalid gameTime");
                return;
            }
             
            BServer.s_boggleDictionary = new HashSet<string>(System.IO.File.ReadAllLines(args[1])); //Load our dictionary into a HashSet

            BServer server;

            if (args.Length > 2)
            {
                string boardString = args[2];
                if (boardString.Length != 16)
                {
                    Console.WriteLine("Incorrect arguments - invalid starting board"); //Argument checking
                    return;
                }

                server = new BServer(gameLength, args[2]); //Start a server with the specified board
            }
            else
                server = new BServer(gameLength); //Start server with a random board.

            Console.WriteLine("Boggle server started.");

            BoggleWebServer webServer = new BoggleWebServer();
            Thread webThread = new Thread(() => { webServerConnectionLoop(webServer); });
            webThread.Start();

            while(true)
               server.acceptConnections(); //Tell the server to continually check for connections. AccpetConnections is blocking.
        }

        /// <summary>
        /// Continually check for connections to the webserver via while loop
        /// BoggleWebServer.AcceptConnections is blocking
        /// </summary>
        /// <param name="webServer"></param>
        public static void webServerConnectionLoop(BoggleWebServer webServer)
        {
            while(true)
            {
                //continually check for web connections
                webServer.acceptConnections(); //blocking method
            }
        }
    }
}
