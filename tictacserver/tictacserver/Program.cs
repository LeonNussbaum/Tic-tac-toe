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
                int index = 0;
                for (int i = 0; i < Program.gameinstances.Count;)
                {
                    if (Program.gameinstances[i].id == e.Data.Substring(1))
                    {
                        index = i; break;
                    }
                }
                if (Program.gameinstances[index].Playercount == 2)
                {
                    
                }
                else
                {
                    string user = "";
                    if (Program.gameinstances[index].playerxiset)
                    {
                        user = "O";
                        SendMessageToConnection(Program.gameinstances[index].playerxx.internalid, "START" + Program.gameinstances[index].grid);
                        Send("C" + user);
                        Send("START" + Program.gameinstances[index].grid);
                    }
                    else
                    {
                        user = "X";
                        Send("C" + user);
                    }
                }
                Program.gameinstances[index].Join(new User(Context.UserEndPoint.ToString()));
                Console.WriteLine("[SERVER] Player Joined Session " + Program.gameinstances[index].id);

            }
            else if (e.Data.StartsWith("P"))
            {
                Send(Gameinstance.GetCurrentSessionsById());
                Console.WriteLine("[SERVER] Send sessions to user");
            }
            else
            {
                
            }

            //Send(Context.UserEndPoint.ToString());

            // Example: Sending a message to a specific connection
            //SendMessageToConnection("[::1]:50095", "Your message here");
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
                    Console.WriteLine("[SERVER] Connection is not open.");
                }
            }
            else
            {
                Console.WriteLine("[SERVER] Connection ID not found.");
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
