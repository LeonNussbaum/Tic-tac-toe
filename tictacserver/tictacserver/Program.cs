using WebSocketSharp;

using WebSocketSharp.Server;
using System.Collections.Generic;
namespace tictacserver
{

    public class Join : WebSocketBehavior
    {
        private static Dictionary<string, Join> connections = new Dictionary<string, Join>();

        protected override void OnMessage(MessageEventArgs e)
        {
            if (e.Data.StartsWith("C"))
            {
                Program.gameinstances.Add(new Gameinstance());
                Console.WriteLine("[SERVER] NEW GAME INSTANCE CREATED " + Program.gameinstances[Program.gameinstances.Count-1].id);
            }
            else if (e.Data.StartsWith("J"))
            {
                // Do something for messages starting with "J"
            }
            else if (e.Data.StartsWith("P"))
            {
                Send(Gameinstance.GetCurrentSessionsById());
            }
            else
            {
                // Handle other messages or perform additional checks
            }

            // Send the connection ID to the client
            Send(Context.UserEndPoint.ToString());

            // Example: Sending a message to a specific connection
            SendMessageToConnection("[::1]:50095", "Your message here");
        }

        protected override void OnOpen()
        {
            string connectionId = Context.UserEndPoint.ToString();
            connections.Add(connectionId, this);
        }

        // Method to send a message to a specific connection
        private void SendMessageToConnection(string connectionId, string message)
        {
            if (connections.ContainsKey(connectionId))
            {
                Join connection = connections[connectionId];
                if (connection.Context.WebSocket.IsAlive)
                {
                    connection.Send(message);
                }
                else
                {
                    Console.WriteLine("Connection is not open.");
                }
            }
            else
            {
                Console.WriteLine("Connection ID not found.");
            }
        }
    }

    internal class Program
    {
        public static List<Gameinstance> gameinstances = new List<Gameinstance>();
        static void Main(string[] args)
        {
            WebSocketServer server = new WebSocketServer("ws://localhost:4242");
            server.Start();

            server.AddWebSocketService<Join>("/join");

            Console.WriteLine("[SERVER] Started!");
            Console.ReadLine();
            server.Stop();
        }
    }
}
